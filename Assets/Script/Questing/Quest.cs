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
    public Item ItemReward;
    public bool Completed;

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
        if (ItemReward != null)
            //InventoryController.Instance.GiveItem(ItemReward);
            //GameManager.instanza.inventario[GameManager.instanza.inventario.Keys.Max()+1]=ItemReward;
            //GameManager.instanza.player.inventario.AddItem(ItemReward,1);       // Da rendere scalabile per più di un'istanza dell'oggetto
            GameManager.instanza.menuDiPausa.AggiungiOggetto(ItemReward,1);
            
            GameManager.instanza.player.GuadagnaEsperienza(ExperienceReward);
            int nuovoValore=CoinsReward + int.Parse(GameManager.instanza.stats["Monete"]);
            GameManager.instanza.stats["Monete"]=nuovoValore.ToString();
    }
}
