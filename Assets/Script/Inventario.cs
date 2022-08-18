 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario
{
    public List<Item> itemList;

    public Inventario(){
        itemList = new List<Item>();
        for (int i = 0; i < 20; i++)
        {
            itemList.Add(null);
        }
    }

    public void AddItem(Item item, int quantità){
        
        bool trovato=false;
        
        if (item.isStackable){    
            foreach (Item currentItem in itemList){
                if (currentItem!=null){
                    if (currentItem.name==item.name){
                        currentItem.quantità+=quantità;
                        trovato=true;
                    }
                }
            }
        }

        if (!trovato)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i]==null)
                {
                    item.quantità=quantità;
                    itemList[i]=item;
                    return;
                }
            }
            
            Debug.Log("Inventario pieno");
        }
        
        


        /*
                if (item.isStackable)
                {
                    bool trovato=false;
                    foreach (Item currentItem in itemList)
                    {
                        if (currentItem.name==item.name)
                        {
                            currentItem.quantità+=item.quantità;
                            trovato=true;
                        }
                    }
                    if (!trovato)
                    {
                        itemList[i]=item;
                        //itemList.Add(item);
                    }
                }
                else{
                    itemList[i]=item;
                    //itemList.Add(item);
                }       
        */

    }
}
