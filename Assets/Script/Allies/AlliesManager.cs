using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlliesManager : MonoBehaviour
{
    public FightersListSO AllPossibleAllies;

    [Space(10)]

    public int MaxAlliesNumberAvailable = 10;
    
    private List<Ally> ActiveAllies = new();
    private List<Ally> NotActiveAllies = new ();

    [Space(10)]

    public List<Vector2> AlliesStartingPositions = new();
    
    
    private string saveDirectory;
    private string savePath;

    private void Awake()
    {
        this.saveDirectory = $"{Application.persistentDataPath}/SaveFiles";
        this.savePath = $"{saveDirectory}/AlliesData.json";
        
        this.LoadAlliesStatus();
    }

    public void SpawnAllActiveAllies()
    {
        var alliesStartingPositionsToPop = this.AlliesStartingPositions;
        
        foreach (var ally in this.ActiveAllies)
        {
            var rndIndex = Random.Range(0, alliesStartingPositionsToPop.Count);
            var rndPosition = alliesStartingPositionsToPop[rndIndex];
            alliesStartingPositionsToPop.RemoveAt(rndIndex);

            Instantiate(ally.gameObject, rndPosition, Quaternion.identity);
        }
    }

    public void RemoveAllyFromActiveAllies(Ally allyToRemove)
    {
        this.ActiveAllies.Remove(allyToRemove);
        this.NotActiveAllies.Add(allyToRemove);
        
        this.SaveAlliesStatus();
    }

    public void AddAllyToActiveAllies(Ally allyToAdd)
    {
        this.ActiveAllies.Add(allyToAdd);
        this.NotActiveAllies.Remove(allyToAdd);
        
        this.SaveAlliesStatus();
    }

    public void AddNewAlly(Ally ally)
    {
        this.NotActiveAllies.Add(ally);
        
        this.SaveAlliesStatus();
    }

    private void SaveAlliesStatus()
    {
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
        
        var totalAlliesList = new List<List<Ally>> {this.ActiveAllies,this.NotActiveAllies};
        
        File.WriteAllText(savePath,JsonUtility.ToJson(totalAlliesList));
    }

    private void LoadAlliesStatus()
    {
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
            return;
        }
        
        var alliesDataJson = File.ReadAllText(savePath);
        var alliesData = JsonUtility.FromJson<List<List<Ally>>>(alliesDataJson);
    }
}
