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

    private TMPro.TextMeshProUGUI testoNomeLivello;

    private Image spriteArma;
    private TMPro.TextMeshProUGUI nomeArma;
    private TMPro.TextMeshProUGUI bonusArma;

    private Image[] abilità = new Image[5];
    public Transform cellaAbilità;
    public Transform containerAbilità;

    private TMPro.TextMeshProUGUI testoMonete;

    private Inventario inventario;
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

        //Inventario
        testoMonete=GameObject.Find("TestoMonete").GetComponent<TMPro.TextMeshProUGUI>();
        InizializzaInventario();


        AggiornaContenuto();
    }

    public void MostraMenuDiPausa(){
        AggiornaContenuto();
        //                                      // Non metto il timescale qui perchè altrimenti freeza subito tutto e non parte l'animazione, anche metterlo dopo è uguale, devo aspettare che l'animazione sia finita
        anim.SetTrigger("ApriMenu");            // Faccio partire l'animazione di entrata del menu
        attivo=true;
    }

    public void NascondiMenuDiPausa(){
        Time.timeScale=1;                       // Rimetto il timescale a 1 altrimenti non può fare l'animazione e rimane tutto bloccato
        anim.SetTrigger("ChiudiMenu");          // Faccio partire l'animazione di uscita del menu
        attivo=false;
    }

    private void AggiornaContenuto(){
        // Nome e livello
        testoNomeLivello.text= /*testoNomeLivello.text*/ "Nemaco - LV " + GameManager.instanza.stats["LV"];

        // Arma e statistiche
        nomeArma.text=GameManager.instanza.player.transform.GetChild(0).GetChild(0).GetComponent<Weapon>().nomeArma;
        bonusArma.text="ATK Bonus " + (GameManager.instanza.player.transform.GetChild(0).GetChild(0).GetComponent<Weapon>().baseDamage-1)*100 +"%";
        spriteArma.sprite=GameManager.instanza.player.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite;

        // Abilità

        for (int i = 0; i < GameManager.instanza.player.skillSet.Count; i++)
        {
            RectTransform goRectTransform=GameObject.Find("CellaAbilità " + (i+1)).GetComponent<RectTransform>();

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

        // Inventario        
        testoMonete.text= /*testoNomeLivello.text*/ "Monete: " + GameManager.instanza.inventario["Monete"];

        inventario=GameManager.instanza.player.inventario;    //da cambiare e gestire tutto con l'inventario del game manager (per ora i due sono slegati)

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
    }

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
            cellaAbilitàRectTransform.anchoredPosition = new Vector2(x*dimensioneCella + (x)*14.0f + 17.0f , -10.0f);
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
            cellaInventarioRectTransform.anchoredPosition = new Vector2(x*dimensioneCella + (x)*14.0f + 17.0f ,y*dimensioneCella + (y)*6.0f - (50.0f+13.0f));

            x++;
            if (x>=5){
                x=0;
                y--;
            }
            
        }
    }

    public void EquipaggiaArma(int indice){
        Debug.Log("Equipaggiata arma nello slot " + (indice+1));
        
        Item temp=GameManager.instanza.itemList.Find(x => x.name==GameManager.instanza.player.transform.GetChild(0).GetChild(0).GetComponent<Weapon>().nomeArma);

        GameManager.instanza.player.transform.GetChild(0).GetChild(0).GetComponent<Weapon>().nomeArma=inventario.itemList[indice].name;
        GameManager.instanza.player.transform.GetChild(0).GetChild(0).GetComponent<Weapon>().baseDamage=inventario.itemList[indice].feature;
        GameManager.instanza.player.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite=Resources.Load("IconeOggetti/"+inventario.itemList[indice].spriteName) as Sprite;

        inventario.itemList[indice]=temp;
        
        AggiornaContenuto();
    }

    public void ConsumaOggetto(int indice){
        Debug.Log("Consumato oggetto nello slot " + (indice+1));

        inventario.itemList[indice].quantità--;

        if (inventario.itemList[indice].name=="Cristiani"){
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

    public void MoveItem(int posizioneIniziale, int posizioneFinale){
        inventario.MoveItem(posizioneIniziale,posizioneFinale);
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

    private void Update(){
        if (Input.GetKeyDown(KeyCode.P)){
            if(!attivo){
                MostraMenuDiPausa();
            }
            else{
                NascondiMenuDiPausa();
            }
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attivo") & attivo){
            Time.timeScale=0;                   // Se l'animazione "Attivo" è in corso allora l'apertura del menù è finita, non faccio lo stesso con "Inattivo" perchè con il inattivo viene dopo l'animazione di uscita ma se il timescale è a 0 non parte proprio l'animazione d''uscita e rimane quindi bloccato
        }
    }
}
