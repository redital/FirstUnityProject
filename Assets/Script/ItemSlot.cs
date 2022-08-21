using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData){
        Debug.Log(transform.name);
        if (eventData.pointerDrag != null){

            
            //GameObject oggettoDroppato = eventData.pointerDrag;
            GameObject cellaDiPartenza = eventData.pointerDrag.transform.parent.gameObject;
            GameObject oggettoDroppato = cellaDiPartenza.transform.GetChild(0).gameObject;
            GameObject quantitàOggettoDroppato = cellaDiPartenza.transform.GetChild(1).gameObject;
            
            GameObject cellaDestinazione = this.gameObject;
            GameObject oggettoInDestinazione = cellaDestinazione.transform.GetChild(0).gameObject;
            GameObject quantitàOggettoInDestinazione = cellaDestinazione.transform.GetChild(1).gameObject;

            oggettoDroppato.GetComponent<DragAndDrop>().ResetCanvasGroup();

            //Debug.Log("da " + (int.Parse(cellaDiPartenza.name.Split(" ")[1])) + " a " + (int.Parse(cellaDestinazione.name.Split(" ")[1])));
            
            if (oggettoDroppato.transform.parent.name.Split(" ")[0]=="CellaInventario")
            {
                GameManager.instanza.menuDiPausa.MoveItem(int.Parse(cellaDiPartenza.name.Split(" ")[1])-1,int.Parse(cellaDestinazione.name.Split(" ")[1])-1);
            }
            
            if (oggettoDroppato.transform.parent.name.Split(" ")[0]=="CellaAbilità")
            {
                GameManager.instanza.menuDiPausa.MoveSkill(int.Parse(cellaDiPartenza.name.Split(" ")[1])-1,int.Parse(cellaDestinazione.name.Split(" ")[1])-1);
            }
        }
    }
}
