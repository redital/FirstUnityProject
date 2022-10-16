using UnityEngine;

public abstract class GenericMenuPanel : MonoBehaviour
{
    public GenericMenuPanel PanelsToMoveForSpace;
    public GenericMenuPanel PanelsToClose;

    public abstract void OpeningAnimation();
    public abstract void ClosingAnimation();
}
