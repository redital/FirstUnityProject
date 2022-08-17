using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    public string nomeArma = "Spada di legno";

    public float baseDamage = 1;          //Danno base dovuto all'arma, il danno totale è calcolato come AttaccoBase*ATKAttaccante/DEFRicevennte
    public float pushForce = 2.0f;

    private float cooldown = 0.25f;      //Si può seferrare un attacco ogni <cooldown> secondi (questo anche e sopratutto perchè l'animazione dura <cooldown> secondi (non è automatica la cosa))
    private float lastSwing;

    private Animator anim;

    protected override void Start(){
        base.Start();
        anim=GetComponent<Animator>();
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
        attackPower = baseDamage*transform.parent.GetComponent<Fighter>().ATK,  //Viene recuperato il valore della statistica di attacco dell'attaccante per il calcolo della potenza d'attacco
        origin = transform.position,
        pushForce = pushForce
        };

        //GameManager.instanza.MostraFloatingText(dmg.damageAmount.ToString(), transform.position);
        coll.SendMessage("RecivedDamage",dmg);

        //Ogni volta che un colpo va a segno si recuperano un tot di PA
        if (Time.time - lastSwing > cooldown){
            lastSwing = Time.time;
            transform.parent.GetComponent<Fighter>().PARecovery+=transform.parent.GetComponent<Fighter>().PARecoveryAmount;
        }
        
    }

    //Agita la spada, il collider è gestito durante l'animazione: attivato all'inizio e disattivato alla fine
    public void Swing (){
        if (Time.time - lastSwing > cooldown){
            lastSwing = Time.time;
            anim.SetTrigger("Swing");
        }
    }

    public void Skill (string skillName){
        anim.SetTrigger(skillName);
    }

}
