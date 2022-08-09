using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : Fighter
{
    public int PA;
    public int PAMAX;
    protected override void Start(){
        base.Start();                   // Per non perdere l'assegnazione del BoxCollider (da Mover) e dell'arma (da Fighter)
        // Carico le statistiche salvate
        PVMAX=int.Parse(GameManager.instanza.stats["PVMAX"]);
        PAMAX=int.Parse(GameManager.instanza.stats["PAMAX"]);

        PV=int.Parse(GameManager.instanza.stats["PV"]);
        PA=int.Parse(GameManager.instanza.stats["PA"]);
        
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

        //Aggiorno, altrimenti quando salvo non salva questi cambiamenti
        GameManager.instanza.stats["PV"]=PV.ToString();
        GameManager.instanza.stats["PA"]=PA.ToString();

        /*
        Le funzioni Update (e le sue varianti) vengono richiamate ad ogni frame, gestiamo quindi qui tutti gli input.
        */
        
    }
}
