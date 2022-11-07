using DG.Tweening;
using UnityEngine;

public class MainMenuPanel : GenericMenuPanel
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !GameManager.instanza.staParlando)
        {
            if (!this.IsPanelOpen)
            {
                this.OpeningAnimation();
                GameManager.instanza.FermaGioco();
            }
            else
            {
                this.ClosingAnimation();
                GameManager.instanza.RiprendiGioco();
            }
        }
    }

    protected override void OpeningAnimationCore()
    {
        transform.DOLocalMove(new Vector3(0, -955, 0), 0.7f);

        foreach (var panel in this.PanelsToMove)
        {
            var panelTransform = panel.transform;
            var localPanelPosition = panelTransform.localPosition;
            
            panelTransform.DOLocalMove(new Vector3(localPanelPosition.x, -955, localPanelPosition.z), 1);
        }

    }

    protected override void ClosingAnimationCore()
    {
        gameObject.transform.DOLocalMove(new Vector3(0, 0, 0), 0.7f);

        
        foreach (var panel in this.PanelsToMove)
        {
            var panelTransform = panel.transform;
            var localPanelPosition = panelTransform.localPosition;
            
            panelTransform.DOLocalMove(new Vector3(localPanelPosition.x, 0, localPanelPosition.z), 1);
        }
        
        
    }
}
