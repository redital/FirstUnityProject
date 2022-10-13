using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fighterslist", menuName = "Lists/FightersList", order = 1)]
public class FightersListSO : ScriptableObject
{
    public List<Fighter> List = new();
}
