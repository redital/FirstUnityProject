using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericMenuPanel : MonoBehaviour
{
    public List<GenericMenuPanel> PanelsToMove;
    public List<GenericMenuPanel> PanelsToClose;

    public bool IsPanelOpen;

    protected abstract void OpeningAnimationCore();
    protected abstract void ClosingAnimationCore();

    public void OpeningAnimation()
    {
        this.OpeningAnimationCore();
        this.IsPanelOpen = true;
    }

    public void ClosingAnimation()
    {
        this.ClosingAnimationCore();
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
            panel.ClosingAnimationCore();
        }
    }
}
