using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Chest : Collectable
{
    public Sprite fullChest;    // Quando il giocatore passa su di una chest chiusa questa si apre ed è piena
    public Sprite emptyChest;   // se prende il contenuto la chest rimane aperta ma vuota
    private Sprite chest;       // se se ne va senza prendere il contenuto la chest si richiude
    
    public int coinsAmmount;

    protected override void Start(){
        base.Start();
        chest=GetComponent<SpriteRenderer>().sprite;
    }

    /*
    Chest è figlia di Collectable che è figlia di Collidable, eredita quindi OnCollide da Collidable e OnCollect da Collectable
    */

    // Quando il giocatore tocca la chest questa, se non è già stata depredata, si apre
    protected override void OnCollide(Collider2D coll){
        base.OnCollide(coll);
        if(coll.name==GameManager.instanza.player.name & collected==false){
            GetComponent<SpriteRenderer>().sprite = fullChest;
        }
    }

    // Se il giocatore è a contatto con la chest può depredarla, se nonn è già stato fatto, premendo la barra spaziatrice
    protected override void OnCollect(){
        if (Input.GetKeyDown(KeyCode.Space) & !GameManager.instanza.combatStatus){
            if (collected==false){
                collected=true;

                // Informo il giocatore con un FloatingText 
                GameManager.instanza.MostraFloatingText("+" + coinsAmmount + " monete!", position:transform.position + new Vector3 (0,0.2f,0), motion:Vector3.up*25, color:Color.yellow);

                // Aggiorno l'inventario
                int nuovoValore=coinsAmmount + int.Parse(GameManager.instanza.stats["Monete"]);
                GameManager.instanza.stats["Monete"]=nuovoValore.ToString();
                

                GetComponent<SpriteRenderer>().sprite = emptyChest;
            }
        }
    }


    protected override void Update(){
        if(!collected){
            GetComponent<SpriteRenderer>().sprite = chest;
        }
        base.Update();
    }
}
