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
}
