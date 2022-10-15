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

    public List<Vector3> AlliesStartingPositionsOffsetFromPlayer = new();

    [field:SerializeField]
    public bool AreAlliesSpawned { get; private set; }
    
    
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

    public void SpawnActiveAllies()
    {
        if (this.AreAlliesSpawned)
        {
            Debug.Log("Allies already spawned");
            return;
        }
        
        var alliesStartingPositionsToPop = this.AlliesStartingPositionsOffsetFromPlayer;
        
        foreach (var ally in this.ActiveAllies)
        {
            var rndIndex = Random.Range(0, alliesStartingPositionsToPop.Count);
            var rndPosition = alliesStartingPositionsToPop[rndIndex];
            alliesStartingPositionsToPop.RemoveAt(rndIndex);

            Instantiate(ally.gameObject, Player.Instance.transform.position + rndPosition, Quaternion.identity);
        }

        this.AreAlliesSpawned = true;
    }

    public void DestroyActiveAllies()
    {
        if (!this.AreAlliesSpawned)
        {
            Debug.Log("Allies are not spawned");
            return;
        }
        
        foreach (var ally in this.ActiveAllies)
        {
            Destroy(ally.gameObject);
        }

        this.AreAlliesSpawned = false;
    }

    public bool RemoveAllyFromActiveAllies(Alleato allyToRemove)
    {
        if (!this.ActiveAllies.Contains(allyToRemove))
        {
            Debug.Log("Alleato non presente nella lista di alleati attivi");
            return false;
        }
        
        this.ActiveAllies.Remove(allyToRemove);
        this.NotActiveAllies.Add(allyToRemove);

        this.SaveAlliesStatus();
        
        return true;
    }

    public bool AddAllyToActiveAllies(Alleato allyToAdd)
    {
        if (!this.NotActiveAllies.Contains(allyToAdd))
        {
            Debug.Log("Alleato non presente nella lista di alleati inattivi");
            return false;
        }
        
        this.ActiveAllies.Add(allyToAdd);
        this.NotActiveAllies.Remove(allyToAdd);
        
        this.SaveAlliesStatus();

        return true;
    }

    public void AddNewAlly(Alleato ally)
    {
        if (this.ActiveAllies.Count < this.MaxAlliesNumberAvailable)
        {
            this.ActiveAllies.Add(ally);
        }
        else
        {
            this.NotActiveAllies.Add(ally);
        }

        this.SaveAlliesStatus();
    }

    private void SaveAlliesStatus()
    {
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
        
        var totalAlliesList = new List<List<string>> {this.ActiveAllies.ConvertAll(item=>item.gameObject.name),this.NotActiveAllies.ConvertAll(item=>item.gameObject.name)};
        
        File.WriteAllText(savePath,FSUtils.Serialize(totalAlliesList));

        Debug.Log($"Saved allies status to {this.savePath}");
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
        var alliesData = FSUtils.Deserialize<List<List<string>>>(alliesDataJson);

        var activeAlliesNames = alliesData[0];
        var notActiveAlliesNames = alliesData[1];

        foreach (var ally in this.AllPossibleAllies.List)
        {
            if (activeAlliesNames.Contains(ally.gameObject.name))
            {
                this.ActiveAllies.Add(ally.gameObject.GetComponent<Alleato>());
            }
            else if (notActiveAlliesNames.Contains(ally.gameObject.name))
            {
                this.NotActiveAllies.Add(ally.gameObject.GetComponent<Alleato>());
            }
        }

        Debug.Log($"Loaded allies status from {this.savePath}");
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

    [RegisterCommand(Help = "Spawn all the active allies", MinArgCount = 0, MaxArgCount = 0)]
    private static void SpawnActiveAllies(CommandArg[] args)
    {
        AlliesManager.Instance.SpawnActiveAllies();
    }

    [RegisterCommand(Help = "Destroy all the active allies", MinArgCount = 0, MaxArgCount = 0)]
    private static void DestroyActiveAllies(CommandArg[] args)
    {
        AlliesManager.Instance.DestroyActiveAllies();
    }

    [RegisterCommand(Help = "Move an Ally from ACTIVE to INACTIVE - args: AllyPrefabName", MinArgCount = 1, MaxArgCount = 1)]
    private static void AddAllyFromActiveToInactive(CommandArg[] args)
    {
        var allyPrefabName = args[0].String;
        
        foreach (var ally in AlliesManager.Instance.AllPossibleAllies.List)
        {
            if (ally.gameObject.name == allyPrefabName)
            {
                if (AlliesManager.Instance.RemoveAllyFromActiveAllies(ally.gameObject.GetComponent<Alleato>()))
                {
                    Debug.Log($"Moved ally {allyPrefabName} from ACTIVE to INACTIVE");
                }
                else
                {
                    Debug.Log($"Could not move ally {allyPrefabName}");
                }
                
                return;
            }
        }
        
        Debug.Log("Ally not found");
    }
    
    [RegisterCommand(Help = "Move an Ally from INACTIVE to ACTIVE - args: AllyPrefabName", MinArgCount = 1, MaxArgCount = 1)]
    private static void AddAllyFromInactiveToActive(CommandArg[] args)
    {
        var allyPrefabName = args[0].String;
        
        foreach (var ally in AlliesManager.Instance.AllPossibleAllies.List)
        {
            if (ally.gameObject.name == allyPrefabName)
            {
                if (AlliesManager.Instance.AddAllyToActiveAllies(ally.gameObject.GetComponent<Alleato>()))
                {
                    Debug.Log($"Moved ally {allyPrefabName} from INACTIVE to ACTIVE");
                }
                else
                {
                    Debug.Log($"Could not move ally {allyPrefabName}");
                }
                
                return;
            }
        }
        
        Debug.Log("Ally not found");
    }
    
}
