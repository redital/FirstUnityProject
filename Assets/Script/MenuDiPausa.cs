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
    private Sprite noAbilità;

    private TMPro.TextMeshProUGUI testoMonete;



    private void Start(){
        anim=GetComponent<Animator>();
        attivo=false;                           // All'inizio il menu di pausa è chiuso
        testoNomeLivello=GameObject.Find("TestoNomeLivello").GetComponent<TMPro.TextMeshProUGUI>();

        nomeArma=GameObject.Find("NomeArma").GetComponent<TMPro.TextMeshProUGUI>();
        bonusArma=GameObject.Find("BonusArma").GetComponent<TMPro.TextMeshProUGUI>();
        spriteArma=GameObject.Find("SpriteArma").GetComponent<Image>();

        noAbilità=GameObject.Find("Abilità 1").GetComponent<Image>().sprite;
        for (int i = 0; i < 5; i++)
        {
            abilità[i]=GameObject.Find("Abilità " + (i+1)).GetComponent<Image>();
        }

        testoMonete=GameObject.Find("TestoMonete").GetComponent<TMPro.TextMeshProUGUI>();

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
        testoNomeLivello.text= /*testoNomeLivello.text*/ "Nemaco - LV " + GameManager.instanza.stats["LV"];

        nomeArma.text=GameManager.instanza.player.transform.GetChild(0).GetComponent<Weapon>().nomeArma;
        bonusArma.text="ATK Bonus " + (GameManager.instanza.player.transform.GetChild(0).GetComponent<Weapon>().baseDamage-1)*100 +"%";
        spriteArma.sprite=GameManager.instanza.player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

        for (int i = 0; i < 5; i++)
        {
            if (i<GameManager.instanza.player.skillSet.Count){
                if (GameManager.instanza.player.skillSet[i]!=null){
                    abilità[i].sprite=GameManager.instanza.player.skillSet[i].sprite;
                }
                else{
                    abilità[i].sprite=noAbilità;
                }
            }
            else{
                abilità[i].sprite=noAbilità;
            }
        }
        
        testoMonete.text= /*testoNomeLivello.text*/ "Monete: " + GameManager.instanza.inventario["Monete"];

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
