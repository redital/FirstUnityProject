using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using UnityEngine;

public class GenericMenuPanel : MonoBehaviour
{
    [Serializable]
    private struct GenericMenuPanelMoving
    {
        public GenericMenuPanel Panel;
        public Vector2 Position;
    }

    [Serializable]
    private struct GenericMenuPanelAnimation
    {
        public enum Vector2AnimationType
        {
            XOnly,
            YOnly,
            Full
        }

        public Vector2AnimationType AnimationType;
        public Vector2 Vector;
        public float Duration;
    }

    [Title("General Settings")]
    [SerializeField] private bool PauseGameOnOpen;
    [SerializeField] private bool UnpauseGameOnClose;
    [SerializeField] private KeyCode ChangeStatusWithKey;
    
    [Space(20)]
    [Title("Opening Animation Settings")]
    [SerializeField] private GenericMenuPanelAnimation openingAnimation;

    [Space(15)]
    [SerializeField] private GenericMenuPanelMoving[] PanelsToMoveOnOpening;
    [SerializeField] private float PanelsToMoveOnOpeningDuration;
    
    [Space(15)]
    [SerializeField] private List<GenericMenuPanel> PanelsToCloseOnOpening;
    
    
    [Space(20)]
    [Title("Closing Animation Settings")]
    [SerializeField] private GenericMenuPanelAnimation closingAnimation;

    [Space(15)]
    [SerializeField] private GenericMenuPanelMoving[] PanelsToMoveOnClosing;
    [SerializeField] private float PanelsToMoveOnClosingDuration;
    
    [Space(15)]
    [SerializeField] private List<GenericMenuPanel> PanelsToCloseOnClosing;
    
    
    [HideInInspector]
    public RectTransform RectTransform;
    
    [ReadOnly,ShowInInspector,PropertySpace(20)]
    public bool IsOpen { get; private set; }
    
    [ReadOnly,ShowInInspector]
    public bool IsBeingMoved { get; private set; }
    public TweenerCore<Vector2, Vector2, VectorOptions> TweenerCorePerformingTheMotion;
    public bool IsMoving { get; private set; }
    [HideInInspector]
    public GenericMenuPanel PanelPerformingTheMotion;
    private int movingCounter;

    private Vector2 defaultPosition;

    private void Awake()
    {
        this.RectTransform = this.GetComponent<RectTransform>();
        this.defaultPosition = this.RectTransform.anchoredPosition;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(this.ChangeStatusWithKey))
        {
            this.ChangePanelStatus();
        }
    }
    
    /// <summary>
    /// Changes the status of the panel (Open -> Close or Close -> Open)
    /// To be used on a button
    /// </summary>
    public void ChangePanelStatus()
    {
        if (GameManager.instanza.staParlando)
        {
            return;
        }

        if (this.IsOpen)
        {
            this.ClosingAnimation();

            if (this.UnpauseGameOnClose && GameManager.instanza.IsGamePaused)
            {
                GameManager.instanza.RiprendiGioco();
            }
        }
        else
        {
            this.OpeningAnimation();
            
            if (this.PauseGameOnOpen && !GameManager.instanza.IsGamePaused)
            {
                GameManager.instanza.FermaGioco();
            }
            
        }
    }
    

    
#region OpeningAnimation

    private void OpeningAnimation()
    {
        this.IsOpen = true;

        switch (this.openingAnimation.AnimationType)
        {
            case GenericMenuPanelAnimation.Vector2AnimationType.XOnly:
                this.RectTransform.DOAnchorPosX(this.openingAnimation.Vector.x, this.openingAnimation.Duration);
                break;
            
            
            case GenericMenuPanelAnimation.Vector2AnimationType.YOnly:
                this.RectTransform.DOAnchorPosY(this.openingAnimation.Vector.y, this.openingAnimation.Duration);
                break;
            
            
            case GenericMenuPanelAnimation.Vector2AnimationType.Full:
                this.RectTransform.DOAnchorPos(this.openingAnimation.Vector, this.openingAnimation.Duration);
                break;
            
            
            default:
                throw new ArgumentOutOfRangeException();
        }

        this.MovePanelsOnOpening();
        this.ClosePanelsOnOpening();
    }
    
    private void MovePanelsOnOpening()
    {
        foreach (var panelToBeMoved in this.PanelsToMoveOnOpening)
        {
            if (panelToBeMoved.Panel.IsOpen)
            {
                if (panelToBeMoved.Panel.IsBeingMoved)
                {
                    panelToBeMoved.Panel.TweenerCorePerformingTheMotion.Kill();
                }
                
                panelToBeMoved.Panel.IsBeingMoved = true;
                panelToBeMoved.Panel.PanelPerformingTheMotion = this;
                
                this.IsMoving = true;

                panelToBeMoved.Panel.TweenerCorePerformingTheMotion = panelToBeMoved.Panel.RectTransform
                                                                 .DOAnchorPos(new Vector2(panelToBeMoved.Position.x,panelToBeMoved.Position.y), this.PanelsToMoveOnOpeningDuration)
                                                                 .OnComplete( () =>
                                                                 {
                                                                     panelToBeMoved.Panel.IsBeingMoved = false;
                                                                     panelToBeMoved.Panel.TweenerCorePerformingTheMotion = null;
                                                                     panelToBeMoved.Panel.PanelPerformingTheMotion = null;

                                                                     this.movingCounter++;

                                                                     if (this.movingCounter == this.PanelsToMoveOnOpening.Length)
                                                                     {
                                                                         this.movingCounter = 0;

                                                                         this.IsMoving = false;
                                                                     }
                                                                 });
            }
        }
    }
    
    private void ClosePanelsOnOpening()
    {
        foreach (var panel in this.PanelsToCloseOnOpening)
        {
            if (panel.IsOpen)
            {
                panel.ClosingAnimation();   
            }
        }
    }

#endregion OpeningAnimation
    
    

#region ClosingAnimation

    private void ClosingAnimation()
    {
        this.IsOpen = false;

        TweenerCore<Vector2,Vector2,VectorOptions> closingTweenerCore;

        switch (this.openingAnimation.AnimationType)
        {
            case GenericMenuPanelAnimation.Vector2AnimationType.XOnly:
                closingTweenerCore = this.RectTransform.DOAnchorPosX(this.closingAnimation.Vector.x, this.closingAnimation.Duration);
                break;
            
            
            case GenericMenuPanelAnimation.Vector2AnimationType.YOnly:
                closingTweenerCore = this.RectTransform.DOAnchorPosY(this.closingAnimation.Vector.y, this.closingAnimation.Duration);
                break;
            
            
            case GenericMenuPanelAnimation.Vector2AnimationType.Full:
                
                closingTweenerCore = this.RectTransform.DOAnchorPos(this.closingAnimation.Vector, this.closingAnimation.Duration);
                break;
            
            
            default:
                throw new ArgumentOutOfRangeException();
        }

        closingTweenerCore.OnComplete(() =>
        {
            this.RectTransform.anchoredPosition = this.defaultPosition;
        });

        this.MovePanelsOnClosing();
        this.ClosePanelsOnClosing();
    }

    private void ClosePanelsOnClosing()
    {
        foreach (var panel in this.PanelsToCloseOnClosing)
        {
            if (panel.IsOpen)
            {
                panel.ClosingAnimation();   
            }
        }
    }

    private void MovePanelsOnClosing()
    {
        foreach (var panelToBeMoved in this.PanelsToMoveOnClosing)
        {
            if (panelToBeMoved.Panel.IsOpen)
            {
                if (panelToBeMoved.Panel.IsBeingMoved && this != panelToBeMoved.Panel.PanelPerformingTheMotion)
                {
                    return;
                }
                
                panelToBeMoved.Panel.IsBeingMoved = true;
                panelToBeMoved.Panel.PanelPerformingTheMotion = this;
                
                this.IsMoving = true;
                
                panelToBeMoved.Panel.TweenerCorePerformingTheMotion = panelToBeMoved.Panel.RectTransform
                                                                 .DOAnchorPos(new Vector2(panelToBeMoved.Position.x,panelToBeMoved.Position.y), this.PanelsToMoveOnClosingDuration)
                                                                .OnComplete( () =>
                                                                {
                                                                    panelToBeMoved.Panel.IsBeingMoved = false;
                                                                    panelToBeMoved.Panel.TweenerCorePerformingTheMotion = null;
                                                                    panelToBeMoved.Panel.PanelPerformingTheMotion = null;
                                                                    
                                                                    if (this.movingCounter == this.PanelsToMoveOnOpening.Length)
                                                                    {
                                                                        this.movingCounter = 0;

                                                                        this.IsMoving = false;
                                                                    }
                                                                });
            }
        }
    }

#endregion ClosingAnimation
    
}
