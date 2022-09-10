 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario
{
    public List<Item> itemList;
    public int capienza=20;        // Il numero di slot che vogliamo rendere disponibili

    public Inventario(){            // Costruttore
        itemList = new List<Item>();
        // L'inventario viene inizializzato con il numero di slot che vogliamo rendere disponibili
        for (int i = 0; i < capienza; i++)
        {
            itemList.Add(null);
        }
    }

    // Metodo per l'aggiunta di oggetti all'inventario
    public void AddItem(Item item, int quantità){
        
        bool trovato=false;
        // Se l'oggetto è impilabile, cerco una occorrenza di tale oggetto nell'inventario
        if (item.isStackable){    
            foreach (Item currentItem in itemList){
                if (currentItem!=null){
                    if (currentItem.name==item.name){
                        // Se lo trovo aggiorno la quantità dell'occorrenza già presente
                        currentItem.quantità+=quantità;
                        trovato=true;
                        UIEventHandler.ItemAddedToInventory(item);
                        return;
                    }
                }
            }
        }

        // Se non trovo nessuna occorrenza oppure l'oggetto non è impilabile, allora aggiungo l'oggetto 
        // Se l'ogetto non è impilabile, la ricerca precedente non viene eseguita e quindi trovato sarà sicuramente false
        if (!trovato)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i]==null)
                {
                    item.quantità=quantità;
                    itemList[i]=item;
                    UIEventHandler.ItemAddedToInventory(item);
                    return;
                }
            }
            
            // Se l'oggetto viene aggiunto c'è un return, se si arriva qui quindi vuol dire che non è stato possibile aggiungere l'oggetto
            Debug.Log("Inventario pieno");
        }
    }

    public void MoveItem(int posizioneIniziale, int posizioneFinale){
        Item temp = itemList[posizioneIniziale];
        itemList[posizioneIniziale] = itemList[posizioneFinale];
        itemList[posizioneFinale] = temp;
    }
}
