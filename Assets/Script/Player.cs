using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : Fighter
{
    public int PA;
    public int PAMAX;

    private bool usingSkill;
    private float skillLastCasted;
    private int lastSkill;

    protected override void Start(){
        base.Start();                   // Per non perdere l'assegnazione del BoxCollider (da Mover) e dell'arma (da Fighter)
        
        LoadStats();

        skillSet.Add(new Skill{
            name="RotationSkill",
            ATKMultiplier=2.0f,
            DEFMultiplier=1.0f,
            skillDuration=50.0f/60.0f,  //non credo sia la soluzione migliore ma è l'unica che mi viene in mente
            PAConsumati=1
        });
    }

    public void LoadStats(){
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

        if(Input.inputString != "" & !usingSkill){
            int indice;                                                         //numero premuto
            bool is_a_number = Int32.TryParse(Input.inputString, out indice);   //controllo se è effettivamente un numero e nel caso lo memorizzo
            if (is_a_number && indice > 0 && indice <= skillSet.Count){         //se è un numero ed è un numero accettabile (se ho 3 skill e premi 4 non succede nulla)
                Skill skill=skillSet[indice-1];                                 //prendo la skill corrispondente al numero
                if(PA-skill.PAConsumati>=0){
                    PA=PA - skill.PAConsumati;
                    ATK=(int)(ATK*skill.ATKMultiplier);
                    DEF=(int)(DEF*skill.DEFMultiplier);
                    weapon.Skill(skill.name);                                       //performo la skill
                    usingSkill=true;
                    skillLastCasted=Time.time;
                    lastSkill=indice-1;
                }
                
            }
        }

        if(usingSkill){                                                         //Se la skill è finita rimetto i parametri com'erano (sto pensando di cambiarlo, perchè ora come ora se uso una skill passiva non posso usare le altre skill mentre è attiva)
            if(Time.time-skillLastCasted > skillSet[lastSkill].skillDuration){
                ATK=(int)Math.Round(ATK/skillSet[lastSkill].ATKMultiplier);
                DEF=(int)Math.Round(DEF/skillSet[lastSkill].DEFMultiplier);
                usingSkill=false;
            }
        }

        /*
        if (!weapon.GetComponent<BoxCollider2D>().enabled){
            ATK=int.Parse(GameManager.instanza.stats["ATK"]);
        }
        */

        //Aggiorno, altrimenti quando salvo non salva questi cambiamenti
        GameManager.instanza.stats["PV"]=PV.ToString();
        GameManager.instanza.stats["PA"]=PA.ToString();

        /*
        Le funzioni Update (e le sue varianti) vengono richiamate ad ogni frame, gestiamo quindi qui tutti gli input.
        */
        
    }
}
