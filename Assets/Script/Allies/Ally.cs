using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : MonoBehaviour
{
    public FighterConfiguration AllyConfiguration;

    public void SpawnAlly(Vector2 position)
    {
        Instantiate(gameObject,position,Quaternion.identity);
    }
    
}
