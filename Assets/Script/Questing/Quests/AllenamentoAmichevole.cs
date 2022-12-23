using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllenamentoAmichevole : Quest
{
    void Start()
    {
        Debug.Log("Allenamento amichevole");
        QuestName = "Allenamento amichevole";
        Description = "Sconfiggi Alioh";

        frasiAssegnazione = new string[]{"Fra secondo me sei più forte di Alioh", "Fammi vedere come gli fai il culo"};
        frasiConclusione = new string[]{"Sei un grande, è stato fantastico", "Questo è per lo spettacolo che ci hai offerto"};
        frasiIncompleta = new string[]{"Che c'è? Te la stai facendo sotto?"};
        frasiCompleta = new string[]{"Quello sì che è stato uno spettacolo"};

        ItemRewards.Add(GameManager.instanza.itemList.Find(x => x.name=="Pozione"),2);
        ItemRewards.Add(GameManager.instanza.itemList.Find(x => x.name=="Carta da gioco"),3);
        ExperienceReward = 50;
        CoinsReward = 10;
        Goals = new List<Goal>
        {
            new KillGoal(this, "Alioh", "Sconfiggi Alioh", false, 0, 1),
            //new KillGoal(this, 1, "Kill 2 Vampires", false, 0, 2),
            new CollectionGoal(this, "Quadrifoglio", "Find a Log Potion", false, 0, 1,false)
        };

        Goals.ForEach(g => g.Init());
    }

    public override string[] GetFrasiAssegnazione(){
        return frasiAssegnazione;
    }
    public override string[] GetFrasiConclusione(){
        return frasiConclusione;
    }
    public override string[] GetFrasiIncompleta(){
        return frasiIncompleta;
    }
    public override string[] GetFrasiCompleta(){
        return frasiCompleta;
    }
}
