using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : Fighter
{
    protected override void Start(){
        base.Start();                   // Per non perdere l'assegnazione del BoxCollider (da Mover) e dell'arma (da Fighter)
        // Carico le statistiche salvate
        PV=int.Parse(GameManager.instanza.stats["PV"]);
        ATK=int.Parse(GameManager.instanza.stats["ATK"]);
        DEF=int.Parse(GameManager.instanza.stats["DEF"]);
    }

    private void FixedUpdate(){
        //Imposto il movimento del giocatore in base agli input ricevuti
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        
        UpdateMotor(new Vector3(x,y,0));

        //Per attaccare bisogna premere la barra spaziatrice.
        if (Input.GetKeyDown(KeyCode.Space)){
            Attack();
        }

        /*
        Le funzioni Update (e le sue varianti) vengono richiamate ad ogni frame, gestiamo quindi qui tutti gli input.
        */
        
    }
}
