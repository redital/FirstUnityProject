using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuDiPausa : MonoBehaviour
{
    private Animator anim;
    private bool attivo;
    private bool abilitàAppreseAttivo;

    private TMPro.TextMeshProUGUI testoNomeLivello;

    private Image spriteArma;
    private TMPro.TextMeshProUGUI nomeArma;
    private TMPro.TextMeshProUGUI bonusArma;

    public Transform cellaAbilità;
    public Transform containerAbilità;

    public Transform testoAbilità;
    public Transform containerAbilitàApprese;

    private TMPro.TextMeshProUGUI testoMonete;

    private Inventario inventario;
    private Dictionary<string, string> posizioniInventario = new Dictionary<string, string>();
    private Dictionary<string, string> dizionarioInventario = new Dictionary<string, string>();
    public Transform cellaInventario;
    public Transform containerInventario;


    private void Start(){
        anim=GetComponent<Animator>();
        attivo=false;                           // All'inizio il menu di pausa è chiuso
        
        // Nome e livello
        testoNomeLivello=GameObject.Find("TestoNomeLivello").GetComponent<TMPro.TextMeshProUGUI>();

        // Arma e statistiche
        nomeArma=GameObject.Find("NomeArma").GetComponent<TMPro.TextMeshProUGUI>();
        bonusArma=GameObject.Find("BonusArma").GetComponent<TMPro.TextMeshProUGUI>();
        spriteArma=GameObject.Find("SpriteArma").GetComponent<Image>();

        // Abilità
        InizializzaAbilità();
        InizializzaAbilitàAppresa();

        //Inventario
        testoMonete=GameObject.Find("TestoMonete").GetComponent<TMPro.TextMeshProUGUI>();
        InizializzaInventario();


        AggiornaContenuto();
    }

    //-------------------------------------------------Sezione per mostrare e nascondere il menù------------------------------------------------------
    public void MostraMenuDiPausa(){
        AggiornaContenuto();
        //                                         // Non metto il timescale qui perchè altrimenti freeza subito tutto e non parte l'animazione, anche metterlo dopo è uguale, devo aspettare che l'animazione sia finita
        anim.SetTrigger("ApriMenu");               // Faccio partire l'animazione di entrata del menu
        attivo=true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void NascondiMenuDiPausa(){
        GameManager.instanza.RiprendiGioco();      // Rimetto il timescale a 1 altrimenti non può fare l'animazione e rimane tutto bloccato
        if (abilitàAppreseAttivo)
        {
            NascondiAbilitàApprese();
        }
        anim.SetTrigger("ChiudiMenu");             // Faccio partire l'animazione di uscita del menu
        attivo=false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void MostraAbilitàApprese(){
        if (attivo)
        {
            GameManager.instanza.RiprendiGioco(); 
            //                                      // Non metto il timescale qui perchè altrimenti freeza subito tutto e non parte l'animazione, anche metterlo dopo è uguale, devo aspettare che l'animazione sia finita
            anim.SetTrigger("ApriAbilitàApprese");            // Faccio partire l'animazione di entrata del menu
            abilitàAppreseAttivo=true;
        }
        
    }

    public void NascondiAbilitàApprese(){
        if (attivo)
        {
            GameManager.instanza.RiprendiGioco();      // Rimetto il timescale a 1 altrimenti non può fare l'animazione e rimane tutto bloccato
            anim.SetTrigger("ChiudiAbilitàApprese");          // Faccio partire l'animazione di uscita del menu
            abilitàAppreseAttivo=false;            
        }

    }


    //-------------------------------------------------Inizializzazione del contenuto grafico (sono tutte chiamate nello start)
    private void InizializzaAbilità(){

        int x = 0;
        float dimensioneCella = 120f;
        int i=0;
        foreach (Skill skill in GameManager.instanza.player.skillSet)
        {
            i++;
            RectTransform cellaAbilitàRectTransform = Instantiate (cellaAbilità,containerAbilità).GetComponent<RectTransform>();
            cellaAbilitàRectTransform.gameObject.name = "CellaAbilità " + i;
            cellaAbilitàRectTransform.gameObject.SetActive(true);
            cellaAbilitàRectTransform.anchoredPosition = new Vector2(x*dimensioneCella + (x)*14.0f + 17.0f + 10.8f +10.0f, -10.0f -10.0f - 2.57f);
            if (skill!=null){
                cellaAbilitàRectTransform.transform.GetChild(0).gameObject.SetActive(true);
                cellaAbilitàRectTransform.transform.GetChild(0).GetComponent<Image>().sprite=skill.sprite;
                
                cellaAbilitàRectTransform.transform.GetChild(1).gameObject.SetActive(true);
                cellaAbilitàRectTransform.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text=skill.PAConsumati.ToString();
                
            }
            else{
                cellaAbilitàRectTransform.transform.GetChild(0).gameObject.SetActive(false);
                cellaAbilitàRectTransform.transform.GetChild(1).gameObject.SetActive(false);
            }

            x++;
        }
    }

    private void InizializzaAbilitàAppresa(){

        int y = 0;
        float dimensioneCella = 120f;
        int i=0;

        containerAbilitàApprese.GetComponent<RectTransform>().sizeDelta=new Vector2(containerAbilitàApprese.GetComponent<RectTransform>().sizeDelta.x, GameManager.instanza.skillApprese.Count*dimensioneCella + (GameManager.instanza.skillApprese.Count)*6.0f +10.0f);
        foreach (Skill skill in GameManager.instanza.skillApprese)
        {
            i++;
            RectTransform cellaAbilitàRectTransform = Instantiate (cellaAbilità,containerAbilitàApprese).GetComponent<RectTransform>();
            cellaAbilitàRectTransform.gameObject.name = "CellaAbilità " + i;
            cellaAbilitàRectTransform.gameObject.SetActive(true);
            cellaAbilitàRectTransform.anchoredPosition = new Vector2(+17.0f, - y*dimensioneCella - (y)*(6.0f+5.75f) -20.0f -10.0f);
            
            RectTransform testoAbilitàRectTransform = Instantiate (testoAbilità,containerAbilitàApprese).GetComponent<RectTransform>();
            testoAbilitàRectTransform.gameObject.name = "TestoAbilità " + i;
            testoAbilitàRectTransform.gameObject.SetActive(true);
            testoAbilitàRectTransform.anchoredPosition = new Vector2(+17.0f + dimensioneCella + 6.0f, - y*dimensioneCella - (y)*(6.0f+5.75f) -20.0f -10.0f);
            testoAbilitàRectTransform.transform.GetComponent<TMPro.TextMeshProUGUI>().text=skill.name;
            testoAbilitàRectTransform.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text=skill.descrizione;


            if (skill!=null){
                cellaAbilitàRectTransform.transform.GetChild(0).gameObject.SetActive(true);
                cellaAbilitàRectTransform.transform.GetChild(0).GetComponent<Image>().sprite=skill.sprite;
                
                cellaAbilitàRectTransform.transform.GetChild(1).gameObject.SetActive(true);
                cellaAbilitàRectTransform.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text=skill.PAConsumati.ToString();
                
            }
            else{
                cellaAbilitàRectTransform.transform.GetChild(0).gameObject.SetActive(false);
                cellaAbilitàRectTransform.transform.GetChild(1).gameObject.SetActive(false);
            }

            y++;
        }
    }

    private void InizializzaInventario(){
        inventario=GameManager.instanza.player.inventario;

        int x = 0;
        int y = 0;
        float dimensioneCella = 120f;
        int i=0;
        foreach (Item item in inventario.itemList)
        {
            i++;
            RectTransform cellaInventarioRectTransform = Instantiate (cellaInventario,containerInventario).GetComponent<RectTransform>();
            cellaInventarioRectTransform.gameObject.name = "CellaInventario " + i;
            cellaInventarioRectTransform.gameObject.SetActive(true);
            cellaInventarioRectTransform.anchoredPosition = new Vector2(x*dimensioneCella + (x)*14.0f + 17.0f + 10.8f ,y*dimensioneCella + (y)*6.0f - (50.0f+13.0f + 7.15f));

            x++;
            if (x>=5){
                x=0;
                y--;
            }
            
        }
    }

    //-------------------------------------------------Sezione per la gestione del contenuto grafico---------------------------------------------------------
    // Aggiornamento contenuto e/o posizioni contenuto (è il metodo principale che richiama i successivi di questa sezione)
    private void AggiornaContenuto(){
        // Nome e livello
        testoNomeLivello.text= /*testoNomeLivello.text*/ "Nemaco - LV " + GameManager.instanza.stats["LV"];

        // Arma e statistiche
        nomeArma.text=GameManager.instanza.player.transform.GetChild(0).GetChild(0).GetComponent<Weapon>().nomeArma;
        bonusArma.text="ATK Bonus " + (GameManager.instanza.player.transform.GetChild(0).GetChild(0).GetComponent<Weapon>().baseDamage-1)*100 +"%";
        spriteArma.sprite=Resources.Load("ArmiGrandi/"+ GameManager.instanza.player.transform.GetChild(0).GetChild(0).GetComponent<Weapon>().nomeArma.Replace(" ","").Split("_")[0] + "_grande") as Sprite;

        // Abilità

        for (int i = 0; i < GameManager.instanza.player.skillSet.Count; i++)
        {
            RectTransform goRectTransform=GameObject.Find(containerAbilità.gameObject.name + "/CellaAbilità " + (i+1)).GetComponent<RectTransform>();

            if (GameManager.instanza.player.skillSet[i]!=null){
                goRectTransform.transform.GetChild(0).gameObject.SetActive(true);
                goRectTransform.transform.GetChild(0).GetComponent<Image>().sprite=GameManager.instanza.player.skillSet[i].sprite;
                goRectTransform.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
                
                goRectTransform.transform.GetChild(1).gameObject.SetActive(true);
                goRectTransform.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text=GameManager.instanza.player.skillSet[i].PAConsumati.ToString();
                
            }
            else{
                goRectTransform.transform.GetChild(0).gameObject.SetActive(false);
                goRectTransform.transform.GetChild(1).gameObject.SetActive(false);
            }
        }

        // Abilità apprese

        float dimensioneCella=120.0f;
        containerAbilitàApprese.GetComponent<RectTransform>().sizeDelta=new Vector2(containerAbilitàApprese.GetComponent<RectTransform>().sizeDelta.x, GameManager.instanza.skillApprese.Count*dimensioneCella + (GameManager.instanza.skillApprese.Count)*6.0f +10.0f);
        for (int i=0; i <GameManager.instanza.skillApprese.Count;i++)
        {
            if (GameObject.Find(containerAbilitàApprese.gameObject.name + "/CellaAbilità " + (i+1))==null)
            {
                RectTransform cellaAbilitàRectTransform = Instantiate (cellaAbilità,containerAbilitàApprese).GetComponent<RectTransform>();
                cellaAbilitàRectTransform.gameObject.name = "CellaAbilità " + (i+1);
                cellaAbilitàRectTransform.gameObject.SetActive(true);
                cellaAbilitàRectTransform.anchoredPosition = new Vector2(+17.0f, - i*dimensioneCella - (i)*6.0f -10.0f);
                
                RectTransform testoAbilitàRectTransform = Instantiate (testoAbilità,containerAbilitàApprese).GetComponent<RectTransform>();
                testoAbilitàRectTransform.gameObject.name = "TestoAbilità " + (i+1);
                testoAbilitàRectTransform.gameObject.SetActive(true);
                testoAbilitàRectTransform.anchoredPosition = new Vector2(+17.0f + dimensioneCella + 6.0f, - i*dimensioneCella - (i)*6.0f -10.0f);
                testoAbilitàRectTransform.transform.GetComponent<TMPro.TextMeshProUGUI>().text=GameManager.instanza.skillApprese[i].name;
                testoAbilitàRectTransform.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text=GameManager.instanza.skillApprese[i].descrizione;
                
            }
            
            RectTransform goRectTransform=GameObject.Find(containerAbilitàApprese.gameObject.name + "/CellaAbilità " + (i+1)).GetComponent<RectTransform>();
            RectTransform textRectTransform=GameObject.Find(containerAbilitàApprese.gameObject.name + "/TestoAbilità " + (i+1)).GetComponent<RectTransform>();

            if (GameManager.instanza.skillApprese[i]!=null){
                goRectTransform.transform.GetChild(0).gameObject.SetActive(true);
                goRectTransform.transform.GetChild(0).GetComponent<Image>().sprite=GameManager.instanza.skillApprese[i].sprite;
                goRectTransform.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
                
                goRectTransform.transform.GetChild(1).gameObject.SetActive(true);
                goRectTransform.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text=GameManager.instanza.skillApprese[i].PAConsumati.ToString();
                
                
                textRectTransform.transform.gameObject.SetActive(true);
                textRectTransform.transform.GetComponent<TMPro.TextMeshProUGUI>().text=GameManager.instanza.skillApprese[i].name;
                
                textRectTransform.transform.GetChild(0).gameObject.SetActive(true);
                textRectTransform.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text=GameManager.instanza.skillApprese[i].descrizione;

            }
            else{
                goRectTransform.transform.GetChild(0).gameObject.SetActive(false);
                goRectTransform.transform.GetChild(1).gameObject.SetActive(false);

                textRectTransform.transform.gameObject.SetActive(false);
                textRectTransform.transform.GetChild(0).gameObject.SetActive(false);
            }
        
        }
        
        bool finito = false;
        for (int i = GameManager.instanza.skillApprese.Count; i < 50 & !finito; i++)
        {
            if (GameObject.Find(containerAbilitàApprese.gameObject.name + "/CellaAbilità " + (i+1))!=null)
            {
                Destroy(GameObject.Find(containerAbilitàApprese.gameObject.name + "/CellaAbilità " + (i+1)));
                Destroy(GameObject.Find(containerAbilitàApprese.gameObject.name + "/TestoAbilità " + (i+1)));
            }
            else
            {
                finito=true;
            }
        }
        

        // Inventario        
        testoMonete.text= /*testoNomeLivello.text*/ "Monete: " + GameManager.instanza.stats["Monete"];

        AggiornaInventario(inventario);
        inventario=GameManager.instanza.player.inventario;    //da cambiare e gestire tutto con l'inventario dal game manager (per ora i due sono slegati)
        inventario=RiordinaInventario(inventario);
        for (int i = 0; i < inventario.itemList.Count; i++)
        {
            RectTransform goRectTransform=GameObject.Find("CellaInventario " + (i+1)).GetComponent<RectTransform>();

            if (inventario.itemList[i]!=null){
                goRectTransform.transform.GetChild(0).gameObject.SetActive(true);
                goRectTransform.transform.GetChild(0).GetComponent<Image>().sprite=Resources.Load("IconeOggetti/"+inventario.itemList[i].spriteName) as Sprite;
                goRectTransform.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
                
                goRectTransform.transform.GetChild(1).gameObject.SetActive(inventario.itemList[i].isStackable);
                goRectTransform.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text=inventario.itemList[i].quantità.ToString();
                
            }
            else{
                goRectTransform.transform.GetChild(0).gameObject.SetActive(false);
                goRectTransform.transform.GetChild(1).gameObject.SetActive(false);
            }
        }

        SetPosizioniInventario(inventario);
        GameManager.instanza.posizioniInventario=SetPosizioniInventario(inventario);

        AggiornaInventario(inventario);
    }

    private void AggiornaInventario(Inventario inventario){
        GameManager.instanza.inventario=new Dictionary<int, Item>();
        int indice=0;
        foreach (Item item in inventario.itemList)
        {
            if (item!=null)
            {
                GameManager.instanza.inventario[indice]=item;
                indice++;
            }
        }
        GameManager.instanza.player.LoadInventario();
    }

    public void AggiungiOggetto(Item item, int quantità){
        inventario.AddItem(item,quantità); 
        AggiornaInventario(inventario);
    }

    private Inventario RiordinaInventario(Inventario inventario){

        posizioniInventario=GameManager.instanza.posizioniInventario;
        
        Inventario inInventario = new Inventario();
        Inventario fuoriInventario = new Inventario();

        string[] keys = new string[20];
        int k=0;

        // Metto tutti gli oggetti con posizione assegnata nella giusta posizione in un inventario ausiliario
        foreach (string key in posizioniInventario.Keys)
        {
            // Controllo se sono effettivamente nell'inventario (potrebbero essere stati utilizzati nel frattempo)(Per qualche motivo da NullReferenceException...)
            try
            {
                if (inventario.itemList.Find(x => x.name==posizioniInventario[key])!=null){
                    inInventario.itemList[int.Parse(key)-1]=inventario.itemList.Find(x => x.name==posizioniInventario[key]);
                    keys[k]=key;
                    k++;
                }                
            }
            catch{

            }
        }

        // Rimuovo sia dalle posizioniInventario sia dall'inventario gli oggetti appena aggiunti
        for (int j = 0; j < k; j++)
        {
            inventario.itemList.Remove(inventario.itemList.Find(x => x.name==posizioniInventario[keys[j]]));
            posizioniInventario.Remove(keys[j]);
        }

        // Gli oggetti rimanenti sono quelli senza posto assegnato 
        fuoriInventario=inventario;

        // Pulisco le posizioni
        foreach (string key in posizioniInventario.Keys)
        {
            Debug.Log(posizioniInventario[key] + " non trovato nell'inventario. Lo elimino");
        }
        posizioniInventario = new Dictionary<string, string>();

        // Metto gli elementi con posizione assegnate al proprio posto, poi aggiungo gli altri elementi
        inventario = new Inventario();
        for (int i = 0; i < inInventario.itemList.Count; i++)
        {
            if (inInventario.itemList[i]!=null)
            {
                inventario.itemList[i]=inInventario.itemList[i];
            }
        }
        foreach (Item item in fuoriInventario.itemList)
        {
            if (item!=null){
                inventario.AddItem(item, item.quantità);
            }   
        }

        // Stampa di controllo
        /*
        for (int i = 0; i < inventario.itemList.Count; i++)
        {   
            if (inventario.itemList[i]!=null)
            {
                Debug.Log(inventario.itemList[i].name + " in posizione " + i);
            }
            
        }
        */

        return inventario;
    }

    private Dictionary<string,string> SetPosizioniInventario(Inventario inventario){
        Dictionary<string, string> posizioniInventario = new Dictionary<string, string>();
        for (int i = 0; i < inventario.itemList.Count; i++){
            if (inventario.itemList[i]!=null){
                posizioniInventario[(i+1).ToString()]=inventario.itemList[i].name;
            }
        }
        
        return posizioniInventario;
    }

    public void EquipaggiaArma(int indice){
        Debug.Log("Equipaggiata arma nello slot " + (indice+1));
        
        Item temp=GameManager.instanza.itemList.Find(x => x.name==GameManager.instanza.player.transform.GetChild(0).GetChild(0).GetComponent<Weapon>().nomeArma);

        GameManager.instanza.player.transform.GetChild(0).GetChild(0).GetComponent<Weapon>().nomeArma=inventario.itemList[indice].name;
        GameManager.instanza.player.transform.GetChild(0).GetChild(0).GetComponent<Weapon>().baseDamage=inventario.itemList[indice].feature;
        GameManager.instanza.player.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite=Resources.Load("SpriteArmi/"+inventario.itemList[indice].spriteName.Split("_")[0] + "_sprite") as Sprite;

        GameManager.instanza.stats["Arma"]=inventario.itemList[indice].name;

        inventario.itemList[indice]=temp;

        GameManager.instanza.posizioniInventario[(indice+1).ToString()]=temp.name;
        
        AggiornaContenuto();
    }

    public void ConsumaOggetto(int indice){
        Debug.Log("Consumato oggetto nello slot " + (indice+1));

        inventario.itemList[indice].quantità--;

        if (inventario.itemList[indice].name=="Pozione"){
            GameManager.instanza.player.Heal((int)Math.Round(inventario.itemList[indice].feature));
        }
        else{
            Debug.Log("Oggetto specifico non implementato");
        }

        if (inventario.itemList[indice].quantità<1){
            inventario.itemList[indice]=null;
        }
        
        AggiornaContenuto();
    }

    public void EquipaggiaAbilità(int indice){
        Debug.Log("Equipaggiata abilità nello slot " + (indice+1));

        if (!GameManager.instanza.player.skillSet.Contains(null))
        {
            return;
        }

        Skill equipaggiata = GameManager.instanza.skillApprese[indice];
        if (GameManager.instanza.player.skillSet.Contains(equipaggiata))
        {
            return;
        }

        int libero = -1;
        for (int i = GameManager.instanza.player.skillSet.Count-1; i > -1; i--)
        {
            if (GameManager.instanza.player.skillSet[i]==null)
            {
                libero=i;
            }
        }
        if (libero<0)
        {
            return;
        }
        GameManager.instanza.player.skillSet[libero]=equipaggiata;
        
        AggiornaContenuto();
    }

    public void DequipaggiaAbilità(int indice){
        Debug.Log("Dequipaggiata abilità nello slot " + (indice+1));
        GameManager.instanza.player.skillSet[indice]=null;

        AggiornaContenuto();
    }

    public void MoveItem(int posizioneIniziale, int posizioneFinale){
        inventario.MoveItem(posizioneIniziale,posizioneFinale);
        if (inventario.itemList[posizioneIniziale]!=null)
        {
            GameManager.instanza.posizioniInventario[(posizioneIniziale+1).ToString()]=inventario.itemList[posizioneIniziale].name;
        }
        else
        {
            GameManager.instanza.posizioniInventario.Remove((posizioneIniziale+1).ToString());
        }
        
        if (inventario.itemList[posizioneFinale]!=null)
        {
            GameManager.instanza.posizioniInventario[(posizioneFinale+1).ToString()]=inventario.itemList[posizioneFinale].name;
        }
        else
        {
            GameManager.instanza.posizioniInventario.Remove((posizioneFinale+1).ToString());
        }

        AggiornaContenuto();
    }

    public void MoveSkill(int posizioneIniziale, int posizioneFinale){
        Skill temp = GameManager.instanza.player.skillSet[posizioneIniziale];
        GameManager.instanza.player.skillSet[posizioneIniziale] = GameManager.instanza.player.skillSet[posizioneFinale];
        GameManager.instanza.player.skillSet[posizioneFinale] = temp;
        AggiornaContenuto();
    }

    public Item GetItem(int indice){
        return inventario.itemList[indice];
    }

    public Item GetItem(string nome){
        return inventario.itemList.Find(x => x.name==nome);
    }

    public void RemoveItem(string nome){
        inventario.itemList.Find(x => x.name==nome).quantità--;

        if (inventario.itemList.Find(x => x.name==nome).quantità<1){
            inventario.itemList[inventario.itemList.FindIndex(x => x.name==nome)]=null;
        }
    }

    private void Update()
    {
        return;
        if (Input.GetKeyDown(KeyCode.P) & !GameManager.instanza.staParlando){
            
            if(!attivo){
                MostraMenuDiPausa();
            }
            else{
                NascondiMenuDiPausa();
            }
            
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attivo") & attivo & !abilitàAppreseAttivo){
            GameManager.instanza.FermaGioco();                // Se l'animazione "Attivo" è in corso allora l'apertura del menù è finita, non faccio lo stesso con "Inattivo" perchè con il inattivo viene dopo l'animazione di uscita ma se il timescale è a 0 non parte proprio l'animazione d''uscita e rimane quindi bloccato
        }
        
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attivo abilità apprese") & abilitàAppreseAttivo){
            GameManager.instanza.FermaGioco();                   // Se l'animazione "Attivo" è in corso allora l'apertura del menù è finita, non faccio lo stesso con "Inattivo" perchè con il inattivo viene dopo l'animazione di uscita ma se il timescale è a 0 non parte proprio l'animazione d''uscita e rimane quindi bloccato
            anim.ResetTrigger("ApriAbilitàApprese");
        }

        if (!attivo)
        {
            abilitàAppreseAttivo=false;
        }

        GameManager.instanza.menuAperto=attivo;
    }
}
