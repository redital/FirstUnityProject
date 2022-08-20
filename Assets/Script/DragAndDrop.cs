using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerClickHandler,IEndDragHandler,IBeginDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform=GetComponent<RectTransform>();
        canvas=GameObject.Find("Menu di pausa").GetComponent<Canvas>();
        canvasGroup=GetComponent<CanvasGroup>();
    }

    public void OnPointerClick(PointerEventData eventData){
        Debug.Log("Hai cliccato");
        int clickCount = eventData.clickCount;

        if (clickCount == 1)
            OnSingleClick();
        else if (clickCount == 2)
            OnDoubleClick();
    }

    void OnSingleClick()
    {
        Debug.Log("Single Clicked");
    }

    void OnDoubleClick()
    {
        Debug.Log("Double Clicked");
    }

    public void OnBeginDrag(PointerEventData eventData){
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData){
        rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData){
        ResetCanvasGroup();
        rectTransform.anchoredPosition = new Vector2(0,0); //Per qualche motivo questo metodo è richiamato solo quando non c'è uno swap di celle (probabilmente perchè non trova l'oggetto dove lo aveva preso) quindi se non va in nessun'altra cella deve tornare indietro
    }

    public void ResetCanvasGroup(){
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }
}
