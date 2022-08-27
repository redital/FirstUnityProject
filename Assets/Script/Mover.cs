using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Polarith.Utils;

public class Mover : MonoBehaviour
{
    protected Sprite davanti;
    public Sprite dietro;
    public Sprite diLato; 

    public Polarith.AI.Move.AIMContext Context;

    // Movimenti base
    protected BoxCollider2D BoxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;
    protected float speed = 1.5f;

    // Sistema Wandering (cammina in maniera random senza allontanarsi troppo dal punto di partenza)
    protected float wanderingMoveSpeed = 0.16f;
    protected float wanderingMinTime=1.0f, wanderingMaxTime=4.0f;
    protected float wanderingDecisionTimeCount = -1.0f;
    protected float maxWanderingDistance = 0.5f;

    protected Vector3 startingPosition;

    // Possibili direzioni in cui l'oggetto può muoversi. Zero c'è quattro volte per aumentare la probabilità che stia fermo
    protected Vector3[] moveDirections = new Vector3[] {  new Vector3(1.0f,0,0), 
                                                        new Vector3(-1.0f,0,0), 
                                                        new Vector3(0,1.0f,0), 
                                                        new Vector3(0,-1.0f,0), 
                                                        new Vector3(0,0,0), 
                                                        new Vector3(0,0,0), 
                                                        new Vector3(0,0,0), 
                                                        new Vector3(0,0,0)      };
    protected int firstCurrentWanderingMoveDirection = 0;
    protected int secondCurrentWanderingMoveDirection = 0; //uso due scelte per aumentare la varietà di mosse (adesso può andare anche in diagonale e può variare anche la velocità)
    protected Vector3 currentWanderingMove;
    /*  Con le scelte correnti le probabilità sono le seguenti
        *   Oggetto fermo = 25% + 6.25% = 31.25%
        *   Oggetto in movimento nelle 4 direzioni principali (lento) = 50%
        *   Oggetto in movimento nelle 4 direzioni principali (veloce) = 6.25%
        *   Oggetto in movimento in diagonale = 12.5%
    */    

    protected virtual void Start(){
        BoxCollider = GetComponent<BoxCollider2D>();
        
        davanti=GetComponent<SpriteRenderer>().sprite;
        startingPosition=this.transform.position;
    }

    protected void Wandering(){
        currentWanderingMove=moveDirections[firstCurrentWanderingMoveDirection]+moveDirections[secondCurrentWanderingMoveDirection];
        // Bisogna aggiungere che se supera una certa distanza dalla posizione di partenza allora devve tornare indietro
        // la soluzione più semplice a livello di codice probabilmente è se è lontano e questa sclta lo farà allontanare ancora allora cambia scelta
        if(Vector3.Distance(startingPosition,transform.position)>maxWanderingDistance){
            int i=0; //in genere non occorre ma un po' ogni tanto capita che rimanga un po' bloccato, quindi se in 10 iterazioni non riesce non fa niente (ci riproverà al prossimo frame)
            while (Vector3.Distance(startingPosition,transform.position+currentWanderingMove)>=Vector3.Distance(startingPosition,transform.position)& i<10)
            {
                i++;
                ChooseWanderingMoveDirection();
                currentWanderingMove=moveDirections[firstCurrentWanderingMoveDirection]+moveDirections[secondCurrentWanderingMoveDirection];
            }
        }

        // Faccio muovere l'oggetto
        UpdateMotor(currentWanderingMove* wanderingMoveSpeed);
        ComputeNextWanderingMove();
    }

    protected void ComputeNextWanderingMove(){
        if (wanderingDecisionTimeCount > 0) {
            wanderingDecisionTimeCount -= Time.deltaTime;
        }
        else{
            // Setto il tempo per cui durerà la prossima mossa
            wanderingDecisionTimeCount = UnityEngine.Random.Range(wanderingMinTime, wanderingMaxTime);
 
            // Scelgo la direzione in cui si muoverà per i prossimi decisionTimeCount secondi
            ChooseWanderingMoveDirection();
        }
    }
 
    protected void ChooseWanderingMoveDirection()
    {
        // Scelgo randomicamente la direzione tra quelle impostate
        firstCurrentWanderingMoveDirection = Mathf.FloorToInt(UnityEngine.Random.Range(0, moveDirections.Length));
        secondCurrentWanderingMoveDirection = Mathf.FloorToInt(UnityEngine.Random.Range(0, moveDirections.Length));
    }

    protected void OnEnable()
    {
        if (Context == null)
            Context = GetComponentInChildren<Polarith.AI.Move.AIMContext>();
        if (Context == null)
            enabled = false;
    }

    protected void SetSeek(Transform seguiTransform){
        Polarith.AI.Move.AIMSeek[] componenti = GetComponents<Polarith.AI.Move.AIMSeek>();
        for (int i = 0; i < componenti.Length; i++)
        {
            if (componenti[i].Label=="SeekInterest")
            {
                componenti[i].GameObjects.Add(seguiTransform.gameObject);
            }
            if (componenti[i].Label=="SeekDanger")
            {
                Transform collision = GameObject.Find("Collision").transform.GetChild(0).transform;
                for (int j = 0; j < collision.childCount; j++)
                {
                    componenti[i].GameObjects.Add(collision.GetChild(j).gameObject);
                }
            }
            if (componenti[i].Label=="SeekPeople")
            {
                GameObject[] persone =  GameObject.FindGameObjectsWithTag("Actor");
                for (int j = 0; j < persone.Length; j++)
                {
                    componenti[i].GameObjects.Add(persone[j]);
                }
                persone =  GameObject.FindGameObjectsWithTag("Fighter");
                for (int j = 0; j < persone.Length; j++)
                {
                    componenti[i].GameObjects.Add(persone[j]);
                }
            }
        }
    }

    protected void Segui(Transform seguiTransform , float followDistance){
        if (Vector3.Distance(seguiTransform.position,transform.position)>followDistance){
            //transform.GetComponent<Polarith.AI.Move.AIMSimpleController2D>().Speed=1;
            //UpdateMotor((seguiTransform.position - transform.position).normalized);
            UpdateMotor(ContextDecision(Context.DecidedDirection * Context.DecidedMagnitude));
        }    
        else
        {
            //transform.GetComponent<Polarith.AI.Move.AIMSimpleController2D>().Speed=0;
        }
    }

    protected Vector3 ContextDecision(Vector3 direzione){
        if (Mathf2.Approximately(direzione.sqrMagnitude, 0))
            return new Vector3 (0,0,0);
        return direzione;
    }

    protected virtual void UpdateMotor(Vector3 input){
        //Reset moveDelta
        moveDelta=input;

        //flip del personaggio a seconda che si vada a destra o a sinistra
        if (Math.Abs(moveDelta.x)>Math.Abs(moveDelta.y)){
            if(moveDelta.x>0){
                GetComponent<SpriteRenderer>().sprite = diLato;
                transform.localScale = new Vector3(1,1,1);
            }
            if(moveDelta.x<0){
                GetComponent<SpriteRenderer>().sprite = diLato; 
                transform.localScale = new Vector3(-1,1,1);
            }
        }
        else{
            if(moveDelta.y>0){
                GetComponent<SpriteRenderer>().sprite = dietro;
            }
            if(moveDelta.y<0){
                GetComponent<SpriteRenderer>().sprite = davanti;
            }
        }
        

        //controllo collisioni asse y
        hit=Physics2D.BoxCast(transform.position,BoxCollider.size,0,new Vector2(0,moveDelta.y),Mathf.Abs(moveDelta.y*Time.deltaTime*speed),LayerMask.GetMask("Actor","Blocking"));

        if (hit.collider==null){
            transform.Translate(0,moveDelta.y*Time.deltaTime*speed,0);
        }

        //controllo collisioni asse x
        hit=Physics2D.BoxCast(transform.position,BoxCollider.size,0,new Vector2(moveDelta.x,0),Mathf.Abs(moveDelta.x*Time.deltaTime*speed),LayerMask.GetMask("Actor","Blocking"));

        if (hit.collider==null){
            transform.Translate(moveDelta.x*Time.deltaTime*speed,0,0);
        }

        try
        {
            if (this.transform.GetChild(0).name=="WeaponPosition"){
                this.transform.GetChild(0).GetChild(0).GetComponent<Weapon>().UpdateWeaponPosition(input);
            }
        }
        catch {
            
        }
        
    }
}
