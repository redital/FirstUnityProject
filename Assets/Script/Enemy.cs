using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter
{
    public int expVal = 1;              //Esperienza da dare al giocatore quando sconfigge il nemico

    public float triggerLenght = 2;     //Distanza dal personaggio da cui lo si inizia ad inseguire
    public float chaseLenght = 4;       //Distanza dalla posizione iniziale da cui lo si inizia a seguire
    private bool chasing;
    private bool collidingWithPlayer;

    private bool wandering = false;

    private Transform playerTransform;  //Riferimento per trovare la posizione del giocatore (non salvo direttamente la posizione perchè dovrei comunque aggioornarla di volta in volta)

    public ContactFilter2D filter;      //Metodo in base al quale si decreta la collisione
    private BoxCollider2D hitbox;       //Collider del nemico. Rappresenta l'area all'interno della quale il nemico può essere considerato colpito
    private Collider2D [] hits = new Collider2D[10];


    protected override void Start(){
        base.Start();                   // Per non perdere l'assegnazione del BoxCollider (da Mover) e dell'arma (da Fighter)
        playerTransform = GameManager.instanza.player.transform;
        //playerTransform = GameManager.instanza.player.transform;
        //startingPosition=transform.position;
        hitbox = transform.GetComponent<BoxCollider2D>(); //notare che questo parametro è uguale al BoxCollider, lo teniamo perchè potrebbe esserci casi in cui vogliamo una hitbox diversa dal collider che usiamo per il movimento
        // in caso si volesse usare una hitbox diversa si deve creare un GameObject (visto che ogni GameObject può avere un solo collider) e usare GetChild al posto di transform
        SetSeek(playerTransform);
    }
    
    
    // Update is called once per frame
    protected override void FixedUpdate(){
        base.FixedUpdate();
        //Controllo se c'è collisione con il giocatore
        hitbox.OverlapCollider(filter,hits);
        collidingWithPlayer = false;
        for (int i=0; i<hits.Length; i++){
            if (hits[i]!=null){
                if (hits[i].name==GameManager.instanza.player.name)
                {
                    collidingWithPlayer = true;
                }   
            }
            hits[i]=null;
        }
        if (Vector3.Distance(playerTransform.position,transform.position)<0.32f)
        {
            collidingWithPlayer = true;
        }

        //Controllo se il nemico deve inseguire il giocatore o meno
        if(Vector3.Distance(startingPosition,transform.position)<chaseLenght){
            if (Vector3.Distance(playerTransform.position,transform.position)<triggerLenght){
                chasing = true;
                wandering = false;
            }
        }
        else{
            chasing=false;
        }

        //In base a quanto calcolato imposto i movimenti del nemico
        if(chasing){
            if(collidingWithPlayer){
                Attack();
            }
            else{
                Segui(playerTransform, 0);
                //UpdateMotor((playerTransform.position - transform.position).normalized);
            }
        }
        else{
            if(Vector3.Distance(startingPosition,transform.position)>0.01 & !wandering){
                UpdateMotor((startingPosition - transform.position).normalized);
            }
            else{
                wandering=true;
                Wandering();
            }
        }

        //Controllo se il nemico va aggiunto alla lista degli inseguitori
        if(chasing) {
            if(GameManager.instanza.chasingEnemy.Find(x=>x.transform.name==transform.name)==null){
                GameManager.instanza.chasingEnemy.Add(this);
            }
        }
        else{
            if(GameManager.instanza.chasingEnemy.Find(x=>x.transform.name==transform.name)==this){
                GameManager.instanza.chasingEnemy.Remove(this);
            }
        }
    }

    protected override void Death(){
        base.Death();

        CombatEvents.EnemyDied(this); //evento indicante la morte del giocatore

        //Se muore va tolto dalla lista degli inseguitori (se c'è)
        if(GameManager.instanza.chasingEnemy.Find(x=>x.transform.name==transform.name)==this){
            GameManager.instanza.chasingEnemy.Remove(this);
        }

        //Distruggo l'oggetto dalla scena
        Destroy(gameObject);

        GameManager.instanza.player.GuadagnaEsperienza(expVal);
    }
}
