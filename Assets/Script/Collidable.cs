using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour     // Potrebbe essere una classe astratta
{
    public ContactFilter2D filter;                      // Criterio in base al quale viene decretata una collisione
    private BoxCollider2D boxCollider;
    private Collider2D [] hits = new Collider2D[10];    // Posso gestire al massimo 10 collisioni per frame (non so se sia tanto o poco)

    // Start is called before the first frame update
    protected virtual void Start(){
        boxCollider=GetComponent<BoxCollider2D>();        
    }

    // Update is called once per frame
    protected virtual void Update(){
        boxCollider.OverlapCollider(filter,hits);

        // Controllo se ci sono collisioni, in tal caso chiamo OnCollide
        for (int i=0; i<hits.Length; i++){
            if (hits[i]!=null){
                OnCollide(hits[i]);
            }
            hits[i]=null;
        }
    }

    // Metodo vuoto, c'è perchè ogni classe figlia possa farne l'override
    protected virtual void OnCollide(Collider2D coll){
        Debug.Log(coll.name);
    }
}
