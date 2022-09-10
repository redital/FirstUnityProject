using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionGoal : Goal {
    public string itemName { get; set; }

    public CollectionGoal(Quest quest, string itemName, string description, bool completed, int currentAmount, int requiredAmount)
    {
        this.Quest = quest;
        this.itemName = itemName;
        this.Description = description;
        this.Completed = completed;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
    }

    public override void Init()
    {
        base.Init();
        UIEventHandler.OnItemAddedToInventory += ItemPickedUp;      //Non ho aggiunto l'evento da nessuna parte
    }

    void ItemPickedUp(Item item)
    {
        if (item.name == this.itemName)
        {
            Debug.Log("Detected enemy death: " + itemName);
            this.CurrentAmount++;
            Evaluate();
        }
    }

}
