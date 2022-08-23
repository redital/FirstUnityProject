using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    public string nomeArma = "Spada di legno";
    private int lastFrame;
    private int frameAnimazione=15;

    private Vector3 ultimaPosizione = new Vector3(0,0,0);
    private Vector3 ultimaRotazione = new Vector3(0,0,0);

    public float baseDamage = 1.0f;      //Danno base dovuto all'arma, il danno totale è calcolato come AttaccoBase*ATKAttaccante/DEFRicevennte
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
            if (coll.name!=transform.parent.transform.parent.name){
                Attack(coll);
            }
        }
    }

    //Metodo deputato al trasferimento del danno da arma a combattente colpito
    protected virtual void Attack(Collider2D coll){
        Damage dmg = new Damage{
        attackPower = baseDamage*transform.parent.transform.parent.GetComponent<Fighter>().ATK,  //Viene recuperato il valore della statistica di attacco dell'attaccante per il calcolo della potenza d'attacco
        origin = transform.position,
        pushForce = pushForce
        };

        //GameManager.instanza.MostraFloatingText(dmg.damageAmount.ToString(), transform.position);
        coll.SendMessage("RecivedDamage",dmg);

        //Ogni volta che un colpo va a segno si recuperano un tot di PA
        if (Time.time - lastSwing > cooldown){
            lastSwing = Time.time;
            transform.parent.transform.parent.GetComponent<Fighter>().PARecovery+=transform.parent.transform.parent.GetComponent<Fighter>().PARecoveryAmount;
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

    public void UpdateWeaponPosition(Vector3 input){
            float x = input.x;
            float y = input.y;

            if ((x!= 0 | y!=0))
            {
                
                Vector3 traslazione=new Vector3(x,y,0);
                Vector3 rotazione = new Vector3(0,0,0);
                //rotazione = new Vector3(0.0f,0.0f,-Mathf.Atan(x/Mathf.Abs(y))*90.0f);
                
                if (y<0){
                    rotazione = new Vector3(0.0f,0.0f,Mathf.Atan(x/y)*90.0f);
                    rotazione += y*x*(new Vector3(0,0,90.0f));
                }
                else{
                    rotazione = new Vector3(0.0f,0.0f,-Mathf.Atan(x/y)*90.0f);
                    rotazione += y*x*(new Vector3(0,0,20.0f));
                }
                

                Vector3 aggiustamentoRotazione=new Vector3(-(Mathf.Acos(x)-Mathf.PI/2.0f),Mathf.Asin(y),0);

                transform.parent.transform.position=Vector3.Lerp(transform.parent.transform.parent.position + ultimaPosizione , transform.parent.transform.parent.position + Vector3.Normalize((traslazione + aggiustamentoRotazione))/10.0f,(float)lastFrame/frameAnimazione);
                transform.parent.transform.eulerAngles=Vector3.Lerp(ultimaRotazione, rotazione,(float)lastFrame/frameAnimazione);
                if (rotazione == new Vector3 (0,0,0)){
                    if (y<0){
                        transform.parent.transform.position += new Vector3 (0.09f,-0.01f,0);
                    }
                    else{
                        transform.parent.transform.position += new Vector3 (-0.09f,-0.04f,0);
                    }    
                }
                lastFrame++;

                if (lastFrame==frameAnimazione){
                    ultimaRotazione=rotazione;
                    ultimaPosizione=Vector3.Normalize((traslazione + aggiustamentoRotazione))/10.0f;
                    lastFrame=0;
                } 
            }
                       
    }

}
