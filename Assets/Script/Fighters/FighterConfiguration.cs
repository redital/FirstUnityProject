using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FighterConfig", menuName = "Configurations/Fighter Configuration", order = 1)]
public class FighterConfiguration : ScriptableObject
{
    public int Health;
    public int Defence;
    
    [Space(10)]
    
    public int AbilityPoints;
    public float AbilityPointsRecoverForHitAmount;
    
    [Space(10)]
    
    public int Damage;

    [Space(10)]

    public float MovementSpeed;

    [Space(10)]
    
    public int Level;

    [Space(10)]
    public List<Skill> skillSet = new List<Skill>();
}
