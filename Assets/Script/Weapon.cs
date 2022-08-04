using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    public int baseDamage = 1;          //Danno base dovuto all'arma, il danno totale è calcolato come AttaccoBase*ATKAttaccante/DEFRicevennte
    public float pushForce = 2.0f;

    private float cooldown = 0.25f;      //Si può seferrare un attacco ogni <cooldown> secondi (questo anche e sopratutto perchè l'animazione dura <cooldown> secondi (non è automatica la cosa))
    private float lastSwing;

    private Animator anim;

    protected override void Start(){
        base.Start();
        anim=GetComponent<Animator>();
    }
    
    protected override void Update(){
        if (GetComponent<BoxCollider2D>().enabled){
            if (Time.time - lastSwing > cooldown){
                lastSwing = Time.time;
                base.Update();
                Swing();
            }
        }
        /*
        Le funzioni Update (e le sue varianti) vengono richiamate ad ogni frame, gestiamo quindi qui tutti gli input.
        Qui non abbiamo input, l'update viene "attivato" implicitamente dalle classi figlie quando necessario:
        Quando il combattente non intende attaccare il BoxCollider è disabilitato e quindi tutto il codice dell'Update viene ignorato.
        Il criterio in base al quale il BoxCollider viene attivato dipende dalla classe figlia.
        */
    }
    

    //Il BoxCollider è attivo solo quando il combattente intende attaccare, in tal caso se viene effettivamente colpito un altro combattente viene chiamato il metodo Attack
    protected override void OnCollide(Collider2D coll){
        if(coll.tag == "Fighter"){
            if (coll.name!=transform.parent.name){
                Attack(coll);
            }
        }
    }

    //Metodo deputato al trasferimento del danno da arma a combattente colpito
    protected virtual void Attack(Collider2D coll){
        Damage dmg = new Damage{
        attackPower = baseDamage*transform.parent.GetComponent<Fighter>().ATK,  //Viene recuperato il valore della statistica di attacco dell'attaccante per il calcoo della potenza d'attacco
        origin = transform.position,
        pushForce = pushForce
        };

        //GameManager.instanza.MostraFloatingText(dmg.damageAmount.ToString(), transform.position);
        coll.SendMessage("RecivedDamage",dmg);
    }

    //Agita la spada, fatto ciò l'attacco è concluso e quindi il collider disattivato
    private void Swing (){
        Debug.Log("Altro");
        anim.SetTrigger("Swing");
        Debug.Log("Swing");
        GetComponent<BoxCollider2D>().enabled = false;
    }

}
