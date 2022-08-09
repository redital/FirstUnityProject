using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Mover
{
    //Statistiche cambattente
    public int PVMAX = 10;
    public int PV = 10;
    public int ATK=5;
    public int DEF=5;

    public float pushRecoverySpeed = 0.2f;
    
    protected Vector3 pushDirection;

    //Arma equipaggiata
    protected Weapon weapon;

    //Sistema per evitare di rimanere bloccati ed avere un attimo di respiro
    protected float immuneTime = 1.0f;
    protected float lastImmune;

    

    protected override void Start(){
        base.Start();
        weapon = transform.GetChild(0).GetComponent<Weapon>();
    }

    //Il danno è gestito dall'arma(classe Weapon), non dal compattente, il combattente decide erò quando usare l'arma
    protected void Attack(){
        weapon.GetComponent<BoxCollider2D>().enabled = true;
    }
    
    //L'arma avversaria trasferisce il danno (inteso come struct Damage) al combattente con cui collide
    protected virtual void RecivedDamage(Damage dmg){
        if(Time.time-lastImmune>immuneTime){            //Se il tempo d'immunità non è scaduto non succede nulla
            lastImmune = Time.time;
            int damageAmount = (int)Math.Round((float)dmg.attackPower/DEF);     //Il danno è calcolato come AttaccoBase*ATKAttaccante/DEFRicevennte. attackPower è calcolato in Weapon come AttaccoBase*ATKAttaccante
            PV-= damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized*dmg.pushForce;
            
            GameManager.instanza.MostraFloatingText(damageAmount.ToString(),transform.position + new Vector3 (0,0.2f,0), motion:Vector3.up*25,color:Color.red);

            if(PV<1){
                PV=0;
                Death();
            }
        }

        
    }

    protected virtual void Death(){
        GameManager.instanza.MostraFloatingText(this.name + " died!", transform.position - new Vector3 (0,0.2f,0));
    }
    
}
