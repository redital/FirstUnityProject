using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText
{
    public bool attivo;             // Indicatore del se si sta mostrando il testo o meno 
    public GameObject go;           // GameObject della casella di testo contenente per l'appunto il testo
    public Text text;               // Testo pronunciato da chi parla

    public float durata;            // Per quanti secondi il testo deve rimanere sullo schermo 
    public float comparsa;          // Momento in cui si è fatto comparire il testo (per verificare se è finita la durata)
    public Vector3 motion;          // Vettore movimento del testo 
    
    public void MostraTesto(){
        attivo=true;
        comparsa=Time.time;
        go.SetActive(attivo);
    }

    public void NascondiTesto(){
        attivo=false;
        go.SetActive(attivo);
    }

    // Se è finita la durata il testo viene nascosto
    public void UpdateFloatingText(){
        if (attivo){
            if (Time.time - comparsa > durata){
                NascondiTesto();
            }
            
            go.transform.position += motion*Time.deltaTime; // Se si imposta del movimento e il testo è attivo applichiamo il movimento
        }
    }
}
