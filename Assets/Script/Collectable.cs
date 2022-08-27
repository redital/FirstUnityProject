using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Collidable       // Potrebbe essere una classe astratta
{
    protected bool collected;   // Un oggetto può essere raccolto una sola volta, una volta raccolto va segnato così che non venga più raccolto

    /*
    Entrambi i metodi sono fondamentalmente vuoti per essere implementati con override dalle classi figlie
    */
    
    protected override void OnCollide(Collider2D coll){     // Se il giocatore collide con l'oggetto questo viene aiutomaticamente raccolto, se non si intende fare ciò si possono aggiungere condizioni in OnCollect
        if(coll.name==GameManager.instanza.player.name){
            OnCollect();
        }
    }

    protected virtual void OnCollect(){
        collected=true;
    }
}
