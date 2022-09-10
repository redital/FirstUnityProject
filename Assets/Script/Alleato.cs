using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Alleato : Fighter
{
    public float followDistance=1.0f;

    private Transform playerTransform;  //Riferimento per trovare la posizione del giocatore (non salvo direttamente la posizione perchè dovrei comunque aggiornarla di volta in volta)

    public ContactFilter2D filter;      //Metodo in base al quale si decreta la collisione
    private BoxCollider2D hitbox;       //Collider del nemico. Rappresenta l'area all'interno della quale il nemico può essere considerato colpito
    private Collider2D [] hits = new Collider2D[10];

    // Start is called before the first frame update
    protected override void Start(){
        base.Start();                   // Per non perdere l'assegnazione del BoxCollider (da Mover) e dell'arma (da Fighter)
        playerTransform = GameManager.instanza.player.transform;

        SetSeek(playerTransform);

        hitbox = transform.GetComponent<BoxCollider2D>(); //notare che questo parametro è uguale al BoxCollider, lo teniamo perchè potrebbe esserci casi in cui vogliamo una hitbox diversa dal collider che usiamo per il movimento
        // in caso si volesse usare una hitbox diversa si deve creare un GameObject (visto che ogni GameObject può avere un solo collider) e usare GetChild al posto di transform

        OnEnable();
    }


    // Update is called once per frame
    protected override void FixedUpdate(){
        base.FixedUpdate();

        if (!GameManager.instanza.combatStatus){
            Segui(playerTransform , followDistance);
        }
        else
        {
            int scelto=0;
            for (int i = 0; i < GameManager.instanza.chasingEnemy.Count; i++)
            {
                if ((GameManager.instanza.chasingEnemy[i].transform.position-transform.position).sqrMagnitude<(GameManager.instanza.chasingEnemy[scelto].transform.position-transform.position).sqrMagnitude)
                {
                    scelto=i;
                }
            }
            Segui(GameManager.instanza.chasingEnemy[scelto].transform,0.1f);
            //Debug.Log(GameManager.instanza.chasingEnemy[scelto]);

            if ((transform.position - GameManager.instanza.chasingEnemy[scelto].transform.position).sqrMagnitude<0.5f)
            {
                Attack();
            }
        }
        
    }
}
