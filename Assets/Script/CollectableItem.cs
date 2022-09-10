using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : Collectable
{
    [SerializeField]
    private string nome;
    [SerializeField]
    private int quantità;
    
    protected override void OnCollect(){
        collected=true;
        if (GameManager.instanza.itemList.Find(x => x.name==nome)!=null)
        {
            GameManager.instanza.menuDiPausa.AggiungiOggetto(GameManager.instanza.itemList.Find(x => x.name==nome),quantità);
        }
        Destroy(this.gameObject);
    }
}


