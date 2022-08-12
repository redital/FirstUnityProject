using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuDiPausa : MonoBehaviour
{
    private Animator anim;
    private bool attivo;

    public TMPro.TextMeshProUGUI testoNomeLivello;

    private void Start(){
        anim=GetComponent<Animator>();
        attivo=false;                           // All'inizio il menu di pausa è chiuso
        testoNomeLivello=GameObject.Find("TestoNomeLivello").GetComponent<TMPro.TextMeshProUGUI>();
        //GetComponent<TMPro.TextMeshProUGUI>().text
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
