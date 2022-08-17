using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    public Vector3 origin;
    public float attackPower;     //Il danno è calcolato come AttaccoBase*ATKAttaccante/DEFRicevennte. attackPower rappresenta il calcolo parziale AttaccoBase*ATKAttaccante 
    public float pushForce;
}
