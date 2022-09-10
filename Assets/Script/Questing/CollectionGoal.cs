using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionGoal : Goal {
    public string itemName { get; set; }
    
    public bool consegnareLeCose;

    public CollectionGoal(Quest quest, string itemName, string description, bool completed, int currentAmount, int requiredAmount, bool consegnareLeCose)
    {
        this.Quest = quest;
        this.itemName = itemName;
        this.Description = description;
        this.Completed = completed;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
        this.consegnareLeCose = consegnareLeCose;
    }

    public override void Init()
    {
        base.Init();
        try{
            this.CurrentAmount=GameManager.instanza.menuDiPausa.GetItem(itemName).quantità;
            Evaluate();
        }
        catch{
        }
        UIEventHandler.OnItemAddedToInventory += ItemPickedUp;      //Non ho aggiunto l'evento da nessuna parte
    }

    void ItemPickedUp(Item item)
    {
        if (item.name == this.itemName)
        {
            Debug.Log("Detected item collected: " + itemName);
            //this.CurrentAmount+=item.quantità;
            this.CurrentAmount=GameManager.instanza.menuDiPausa.GetItem(itemName).quantità;
            //this.CurrentAmount=int.Parse(GameManager.instanza.inventario[itemName]);
            Evaluate();
        }
    }

}
