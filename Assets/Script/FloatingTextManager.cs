using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer;        // Utilizzo un panel come textContainer, tutti i ConversationText saranno al suo interno, potrebbe anche essere privato, o essere rimosso, da valutare
    public GameObject textPrefab;           // Utilizzo un prefabbricato per creare nuove instanze

    private List<FloatingText> floatingTexts = new List<FloatingText>();    // Lista di oggetti di tipo FloatingText per la gestione tramite apposita classe

     // Metodo per la visualizzazione del FloatingText
    public void MostraTesto(string testo, int fontSize, Color color, Vector3 position, Vector3 motion, float durata){
        FloatingText floatingText = GetFloatingText();
        floatingText.text.text = testo;
        floatingText.text.fontSize = fontSize;
        floatingText.text.color = color;
        floatingText.go.transform.position = Camera.main.WorldToScreenPoint(position);
        floatingText.motion = motion;
        floatingText.durata = durata;

        floatingText.MostraTesto();
    }

    // Getter di un FloatingText non attivo se non esiste o sono tutti attivi ne crea uno nuovo
    private FloatingText GetFloatingText(){
        FloatingText testo = floatingTexts.Find(t => !t.attivo);

        if (testo==null){
            testo = new FloatingText();
            testo.go = Instantiate(textPrefab);
            testo.go.transform.SetParent(textContainer.transform);
            testo.text = testo.go.GetComponent<Text>();

            floatingTexts.Add(testo);
        }

        return testo;
    }

    // Se esistono dei FloatingText li aggiorna tutti
    private void Update(){
        foreach (FloatingText text in floatingTexts){
            text.UpdateFloatingText();            
        }
    }
}
