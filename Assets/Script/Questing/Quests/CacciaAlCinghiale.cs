using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacciaAlCinghiale : Quest
{
    void Start()
    {
        Debug.Log("CacciaAlCinghiale");
        QuestName = "Caccia Al Cinghiale";
        Description = "Vai nella foresta e vai a caccia, il capovillaggio ti ha chiesto di portargli un cinghiale";

        frasiAssegnazione = new string[]{"Eccoti Nemaco, ti stavo aspettando", "Bando alle ciance, voglio che tu vada a caccia di cinchiali", "Non farai molto con quel coltello, tieni una spada"};
        frasiConclusione = new string[]{"Allora ti rendi utile un po' ogni tanto"};
        frasiIncompleta = new string[]{"Allora questo cinghiale?","Sei il solito fannullone"};
        frasiCompleta = new string[]{"Ciao Nemaco, non ti si vede molto in giro ultimamente","Sempre meglio che vederti poltrire tutto il giorno!"};

        GameManager.instanza.menuDiPausa.AggiungiOggetto(GameManager.instanza.itemList.Find(x => x.name=="Spada Goblin"),1);

        ItemRewards.Add(GameManager.instanza.itemList.Find(x => x.name=="Pozione"),2);
        ExperienceReward = 5;
        CoinsReward = 60;
        Goals = new List<Goal>
        {
            new KillGoal(this, "Cinghiale", "Sconfiggi un cinghiale", false, 0, 1),
            //new KillGoal(this, 1, "Kill 2 Vampires", false, 0, 2),
            //new CollectionGoal(this, "Quadrifoglio", "Find a Log Potion", false, 0, 1,false)
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
