using System.Collections.Generic;
using System.IO;
using CommandTerminal;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlliesManager : MonoBehaviour
{
    public static AlliesManager Instance { get; private set; }

    public FightersListSO AllPossibleAllies;

    [Space(10)]
    
    public int MaxAlliesNumberAvailable = 10;
    
    [SerializeField]
    private List<Alleato> ActiveAllies = new();
    [SerializeField]
    private List<Alleato> NotActiveAllies = new ();

    [Space(10)]

    public List<Vector2> AlliesStartingPositions = new();
    
    
    private string saveDirectory;
    private string savePath;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
        
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

    public void RemoveAllyFromActiveAllies(Alleato allyToRemove)
    {
        this.ActiveAllies.Remove(allyToRemove);
        this.NotActiveAllies.Add(allyToRemove);
        
        this.SaveAlliesStatus();
    }

    public void AddAllyToActiveAllies(Alleato allyToAdd)
    {
        this.ActiveAllies.Add(allyToAdd);
        this.NotActiveAllies.Remove(allyToAdd);
        
        this.SaveAlliesStatus();
    }

    public void AddNewAlly(Alleato ally)
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
        
        var totalAlliesList = new List<List<Alleato>> {this.ActiveAllies,this.NotActiveAllies};
        
        File.WriteAllText(savePath,JsonUtility.ToJson(totalAlliesList));
    }
    
    private void LoadAlliesStatus()
    {
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
            return;
        }

        if (!File.Exists(savePath))
        {
            this.SaveAlliesStatus();
            return;
        }
        
        var alliesDataJson = File.ReadAllText(savePath);
        var alliesData = JsonUtility.FromJson<List<List<Alleato>>>(alliesDataJson);
    }
    
    //-------------------------------CHEATS-----------------------------//

    [RegisterCommand(Help = "Add an ally - args: AllyPrefabName", MinArgCount = 1, MaxArgCount = 1)]
    private static void AddAlly(CommandArg[] args)
    {
        var allyPrefabName = args[0].String;

        foreach (var ally in AlliesManager.Instance.AllPossibleAllies.List)
        {
            if (ally.gameObject.name == allyPrefabName)
            {
                AlliesManager.Instance.AddNewAlly(ally.gameObject.GetComponent<Alleato>());
                Debug.Log($"Added ally {allyPrefabName}");
                return;
            }
        }
        
        Debug.Log("Ally not found");
    }
}
