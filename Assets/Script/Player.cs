using System;
using UnityEngine;



public class Player : Fighter
{
    public int EXP;
    public int NextLevelEXP;

    private bool usingSkill;
    private float skillLastCasted;
    private int lastSkill;

    public Inventario inventario;

    public static Player Instance { get; private set; }

    private void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    protected override void Start(){
        base.Start();                   // Per non perdere l'assegnazione del BoxCollider (da Mover) e dell'arma (da Fighter)

        LoadStats();

        LoadInventario();
        
    }

    public void GuadagnaEsperienza(int expVal){
        //Assegno i punti esperienza e lo comunico al giocatore                 //Adesso che ci sono gli eventi potrebbe essere gestito tramite esssi
        GameManager.instanza.MostraFloatingText("+" + expVal.ToString() + " exp", transform.position + new Vector3 (0.2f,0.2f,0), motion:Vector3.up*25, color:Color.blue);
        int nuovoValore=expVal + int.Parse(GameManager.instanza.stats["EXP"]);
        GameManager.instanza.stats["EXP"]=nuovoValore.ToString();
    }

    public void LoadInventario(){
        inventario=new Inventario();
        foreach (int key in GameManager.instanza.inventario.Keys)
        {
            if (GameManager.instanza.itemList.Find(x => x.name==GameManager.instanza.inventario[key].name)!=null)
            {
                inventario.AddItem(GameManager.instanza.itemList.Find(x => x.name==GameManager.instanza.inventario[key].name),GameManager.instanza.inventario[key].quantità);
            }
        }
    }

    public void LoadStats(){
        // Carico le statistiche salvate
        LV=int.Parse(GameManager.instanza.stats["LV"]);
        EXP=int.Parse(GameManager.instanza.stats["EXP"]);
        NextLevelEXP=20*LV;

        PVMAX=int.Parse(GameManager.instanza.stats["PVMAX"]);
        PAMAX=int.Parse(GameManager.instanza.stats["PAMAX"]);

        PV=int.Parse(GameManager.instanza.stats["PV"]);
        PA=int.Parse(GameManager.instanza.stats["PA"]);
        
        ATK=int.Parse(GameManager.instanza.stats["ATK"]);
        DEF=int.Parse(GameManager.instanza.stats["DEF"]);
    }

    protected override void PausableFixedUpdate()
    {
        base.PausableFixedUpdate();

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
                if (skillSet[indice-1]!=null){
                    Skill skill=skillSet[indice-1];                                 //prendo la skill corrispondente al numero
                    if(PA-skill.PAConsumati>=0){
                        PA=PA - skill.PAConsumati;
                        ATK=(int)Math.Round((ATK*skill.ATKMultiplier));
                        DEF=(int)Math.Round((DEF*skill.DEFMultiplier));
                        weapon.Skill(skill.name);                                   //performo la skill
                        usingSkill=true;
                        skillLastCasted=Time.time;
                        lastSkill=indice-1;
                    }
                }    
            }
        }

        //Se la skill è finita rimetto i parametri com'erano (sto pensando di cambiarlo, perchè ora come ora se uso una skill passiva non posso usare le altre skill mentre è attiva)
        if(usingSkill){
            if(Time.time-skillLastCasted > skillSet[lastSkill].skillDuration){
                ATK=(int)Math.Round(ATK/skillSet[lastSkill].ATKMultiplier);
                DEF=(int)Math.Round(DEF/skillSet[lastSkill].DEFMultiplier);
                usingSkill=false;
            }
        }

        //Aggiorno, altrimenti quando salvo non salva questi cambiamenti
        GameManager.instanza.stats["PV"]=PV.ToString();
        GameManager.instanza.stats["PA"]=PA.ToString();
        EXP=int.Parse(GameManager.instanza.stats["EXP"]);
        

        //Se raggiunge il numero di punti esperienza richiesti sale di livello e si aggiornano le informazioni
        if(EXP>=NextLevelEXP){
            LV++;
            EXP-=NextLevelEXP;
            GameManager.instanza.stats["EXP"]=EXP.ToString();
            NextLevelEXP=20*LV;
            GameManager.instanza.stats["LV"]=LV.ToString();
        }

        /*
        Le funzioni Update (e le sue varianti) vengono richiamate ad ogni frame, gestiamo quindi qui tutti gli input.
        */
    }
}
