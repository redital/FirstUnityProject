using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGoal : Goal {
    public string EnemyName { get; set; }

    public KillGoal(Quest quest, string enemyName, string description, bool completed, int currentAmount, int requiredAmount)
    {
        this.Quest = quest;
        this.EnemyName = enemyName;
        this.Description = description;
        this.Completed = completed;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
    }

    public override void Init()
    {
        base.Init();
        CombatEvents.OnEnemyDeath += EnemyDied;
    }

    void EnemyDied(Enemy enemy)
    {
        if (enemy.name == this.EnemyName)
        {
            Debug.Log("Detected enemy death: " + EnemyName);
            this.CurrentAmount++;
            Evaluate();
        }
    }

}
