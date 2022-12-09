using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public abstract class GenericMenuPanel : MonoBehaviour
{
    [Header("Opening Animation")]
    public Vector2 OpeningFinalPosition;
    public float OpeningDuration;
    
    
    [Space(10)]
    [Header("Closing Animation")]
    public Vector2 ClosingFinalPosition;
    public float ClosingDuration;

    [Space(10)]
    [Header("Panels to move when Animating")]
    public SerializedDictionary<GenericMenuPanel,Vector2> PanelsToMove;
    public float PanelsToMoveDuration;
    
    [Space(10)]
    [Header("Panels to close when Animating")]
    public List<GenericMenuPanel> PanelsToClose;

    [HideInInspector]
    public bool IsPanelOpen;
    [HideInInspector]
    public RectTransform RectTransform;

    private void Awake()
    {
        this.RectTransform = this.GetComponent<RectTransform>();
    }

    public void OpeningAnimation()
    {
        this.RectTransform.DOAnchorPos(this.OpeningFinalPosition, OpeningDuration);

        foreach (var panel in this.PanelsToMove)
        {
            var anchoredPosition = panel.Key.RectTransform.anchoredPosition;

            panel.Key.RectTransform.DOAnchorPos(new Vector2(anchoredPosition.x + panel.Value.x,anchoredPosition.y + panel.Value.y), this.PanelsToMoveDuration);
        }
        
        this.IsPanelOpen = true;
    }

    public void ClosingAnimation()
    {
        this.RectTransform.DOAnchorPos(this.ClosingFinalPosition, ClosingDuration);
        
        foreach (var panel in this.PanelsToMove)
        {
            var anchoredPosition = panel.Key.RectTransform.anchoredPosition;

            panel.Key.RectTransform.DOAnchorPos(new Vector2(anchoredPosition.x + panel.Value.x,anchoredPosition.y + panel.Value.y), this.PanelsToMoveDuration);
        }
        
        this.IsPanelOpen = false;
    }

    public void OpenIfCloseCloseIfOpen()
    {
        if (this.IsPanelOpen)
        {
            this.ClosingAnimation();
        }
        else
        {
            this.OpeningAnimation();
        }
    }

    private void ClosingAnimationForPanelsToClose()
    {
        foreach (var panel in this.PanelsToClose)
        {
            panel.ClosingAnimation();
        }
    }
}
