using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC {
    public bool AssignedQuest;
    public bool Helped;

    //[SerializeField]
    private GameObject quests;

    [SerializeField]
    private string questType;
    private Quest Quest;

    protected override void Start(){
        base.Start();
        quests=GameManager.instanza.quests;

        /*
        if (quests.GetComponent<System.Type.GetType(questType)>().gameObject!=null )
        {
            /if ((Quest)quests.GetComponent<System.Type.GetType(questType)>().Completed==false)
            {
                AssignedQuest=true;
                Quest=(Quest)quests.GetComponent<System.Type.GetType(questType)>()
            }
        }
        */
    }
    
    public string[] GetFrasiAssegnazione(){
        return Quest.GetFrasiAssegnazione();
    }
    public string[] GetFrasiConclusione(){
        return Quest.GetFrasiConclusione();
    }
    public string[] GetFrasiIncompleta(){
        return Quest.GetFrasiIncompleta();
    }
    public string[] GetFrasiCompleta(){
        return Quest.GetFrasiCompleta();
    }

    public void AssignQuest()
    {
        AssignedQuest = true;
        Quest = (Quest)quests.AddComponent(System.Type.GetType(questType));
    }

    public bool CheckQuest()
    {
        Quest.CheckGoals();
        if (Quest.Completed)
        {
            Quest.GiveReward();
            Helped = true;
            AssignedQuest = false;
            //DialogueSystem.Instance.AddNewDialogue(new string[] {"Thanks for that! Here's your reward.", "More dialogue"}, name);
        }
        else
        {
            //DialogueSystem.Instance.AddNewDialogue(new string[] {"You're still in the middle of helping me. Get back at it!"}, name);
        }
        return Quest.Completed;
    }
}
