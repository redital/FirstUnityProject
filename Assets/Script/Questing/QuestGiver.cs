using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC {
    public bool AssignedQuest;
    public bool Helped;

    [SerializeField]
    private GameObject quests;

    [SerializeField]
    private string questType;
    private Quest Quest;
    

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
            //DialogueSystem.Instance.AddNewDialogue(new string[] { "You're still in the middle of helping me. Get back at it!"}, name);
        }
        return Quest.Completed;
    }
}
