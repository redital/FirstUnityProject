using System;
using DG.Tweening;
using UnityEngine;

public class AlliesPanel : GenericMenuPanel
{
    protected override void OpeningAnimationCore()
    {
        gameObject.transform.DOMove(new Vector3(-238, 0, 0), 1);

        foreach (var panel in this.PanelsToMove)
        {
            var position = panel.gameObject.transform.position;
            panel.gameObject.transform.DOMove(new Vector3(position.x + 260, position.y, position.z), 1);
        }
    }

    protected override void ClosingAnimationCore()
    {
        gameObject.transform.DOMove(new Vector3(-862, 0, 0), 1);
        
        foreach (var panel in this.PanelsToMove)
        {
            var position = panel.gameObject.transform.position;
            panel.gameObject.transform.DOMove(new Vector3(position.x - 260, position.y, position.z), 1);
        }
    }
}
