using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Mover
{
    public float moveSpeed = 0.08f;
    public float minTime=1.0f, maxTime=4.0f;
    internal float decisionTimeCount = -1.0f;

    // Possibili direzioni in cui l'oggetto può muoversi. Zero c'è due volte per aumentare la probabilità che stia fermo
    internal Vector3[] moveDirections = new Vector3[] { new Vector3(1.0f,0,0), new Vector3(-1.0f,0,0), new Vector3(0,1.0f,0), new Vector3(0,-1.0f,0), new Vector3(0,0,0), new Vector3(0,0,0) };
    internal int currentMoveDirection = 0;

    void Update()
    {

        // Bisogna aggiungere che se supera una certa distanza dalla posizione di partenza allora devve tornare indietro
        // la soluzione più semplice a livello di codice probabilmente è se è lontano e questa sclta lo farà allontanare ancora allora cambia scelta

        // Faccio muovere l'oggetto
        UpdateMotor(moveDirections[currentMoveDirection]* moveSpeed);

        if (decisionTimeCount > 0) {
            decisionTimeCount -= Time.deltaTime;
        }
        else{
            // Setto il tempo per cui durerà la prossima mossa
            decisionTimeCount = Random.Range(minTime, maxTime);
 
            // Scelgo la direzione in cui si muoverà per i prossimi decisionTimeCount secondi
            ChooseMoveDirection();

            
        }
        
    }
 
    void ChooseMoveDirection()
    {
        // Scelgo randomicamente la direzione tra quelle impostate
        currentMoveDirection = Mathf.FloorToInt(Random.Range(0, moveDirections.Length));
    }
}
