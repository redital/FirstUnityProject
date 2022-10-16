using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Fighterslist", menuName = "Lists/FightersList", order = 1)]
public class FightersListSO : ScriptableObject
{
    public List<Fighter> List = new();
    
    [PropertySpace(20)]

    [Button(ButtonSizes.Medium, ButtonStyle.Box, Expanded = true)]
    private void BuildListBasedOnFilter(string filter)
    {
        this.List = new List<Fighter>();
        
        var matchingAssetsGuids = AssetDatabase.FindAssets(filter);

        foreach (var guid in matchingAssetsGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var ally = AssetDatabase.LoadAssetAtPath<Alleato>(path);
            
            if (ally != null)
            { 
                this.List.Add(ally);
            }
        }
    }
    
    [PropertySpace(20)]

    [Button(ButtonSizes.Medium, ButtonStyle.Box, Expanded = true)]
    private void BuildListBasedOnPath(string path)
    {
        this.List = new List<Fighter>();
        
        var matchingAssetsGuids = AssetDatabase.FindAssets("",new []{path});

        foreach (var guid in matchingAssetsGuids)
        {
            var foundPath = AssetDatabase.GUIDToAssetPath(guid);
            var ally = AssetDatabase.LoadAssetAtPath<Alleato>(foundPath);
            
            if (ally != null)
            { 
                this.List.Add(ally);
            }
        }
    }

}
