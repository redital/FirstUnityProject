using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class Quest : MonoBehaviour {
    /*private int level;
    public int Level {
        get
        {
            return level;
        }
        set
        {
            level = value;
        }
    }
    */
    public List<Goal> Goals;
    public string QuestName;
    public string Description;
    public int ExperienceReward;
    public int CoinsReward;
    public Dictionary<Item, int> ItemRewards = new Dictionary<Item, int>();
    public bool Completed;

    public string[] frasiAssegnazione = new string[]{"Fra secondo me sei più forte di Alioh", "Fammi vedere come gli fai il culo"};
    public string[] frasiConclusione = new string[]{"Sei un grande, è stato fantastico", "Questo è per lo spettacolo che ci hai offerto"};
    public string[] frasiIncompleta = new string[]{"Che c'è? Te la stai facendo sotto?"};
    public string[] frasiCompleta = new string[]{"Quello sì che è stato uno spettacolo"};


    public void CheckGoals()
    {
        /*
        Completed = true;
        foreach (Goal g in Goals)
        {
            if (!g.Completed)
            {
                Completed = false;
            }
        }
        */
        Completed = Goals.All(g => g.Completed);
    }

    public void GiveReward()
    {

        foreach (var goal in Goals)
        {
            if (goal.GetType() == typeof(CollectionGoal))
            {
                CollectionGoal cGoal = (CollectionGoal)goal;
                if (cGoal.consegnareLeCose)
                {
                    for (int i = 0; i <= cGoal.RequiredAmount; i++)
                    {
                        GameManager.instanza.menuDiPausa.RemoveItem(cGoal.itemName);
                    }
                }
            }
        }

        if (ItemRewards.Count > 0){
            //InventoryController.Instance.GiveItem(ItemReward);
            //GameManager.instanza.inventario[GameManager.instanza.inventario.Keys.Max()+1]=ItemReward;
            //GameManager.instanza.player.inventario.AddItem(ItemReward,1);
            foreach(KeyValuePair<Item, int> item in ItemRewards)
            {
                GameManager.instanza.menuDiPausa.AggiungiOggetto(item.Key,item.Value);
            }
        }
        GameManager.instanza.player.GuadagnaEsperienza(ExperienceReward);
        int nuovoValore=CoinsReward + int.Parse(GameManager.instanza.stats["Monete"]);
        GameManager.instanza.stats["Monete"]=nuovoValore.ToString();
        
    }
}
