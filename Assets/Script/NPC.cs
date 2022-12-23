using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Mover
{
    protected override void PausableUpdate()
    {   
        Wandering();  
    }

}
