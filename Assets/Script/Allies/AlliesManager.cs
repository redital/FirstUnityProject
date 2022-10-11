using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlliesManager : MonoBehaviour
{
    public List<Ally> ActiveAllies;
    public List<Ally> NotActiveAllies;

    [Space(10)]

    public List<Vector2> AlliesStartingPositions = new();

    public void SpawnAllActiveAllies()
    {
        var alliesStartingPositionsToPop = this.AlliesStartingPositions;
        
        foreach (var ally in this.ActiveAllies)
        {
            var rndIndex = Random.Range(0, alliesStartingPositionsToPop.Count);
            var rndPosition = alliesStartingPositionsToPop[rndIndex];
            alliesStartingPositionsToPop.RemoveAt(rndIndex);

        }
    }

    public void RemoveAllyFromActiveAllies()
    {
        
    }

    public void AddAllyToActiveAllies()
    {
        
    }
}
