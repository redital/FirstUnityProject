using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ConversationTextManager : MonoBehaviour
{
    public GameObject textContainer;        // Utilizzo un panel come textContainer, tutti i ConversationText saranno al suo interno, potrebbe anche essere privato, o essere rimosso, da valutare
    public GameObject textPrefab;           // Utilizzo un prefabbricato per creare nuove instanze

    private ConversationText conversationText;  // Oggetto di tipo ConversationText per la gestione tramite apposita classe

    /*
    è da valutare a struttura di questa classe visto che è stata ottenuta modificando FloatingText, porta tuttavia parecchie differenze 
    importanti come ad esempio il fatto che si possa avere un solo conversation text alla volta. 
    */

    // Metodo per la visualizzazione del ConversationText
    public void MostraTesto(string[] testi, string speaker, int fontSize, Color color, Vector3 position, Vector3 motion){
        ConversationText conversationText = GetConversationText();

        conversationText.frasi = testi;
        conversationText.fraseCorrente = 0;
        conversationText.text.text = testi[conversationText.fraseCorrente];
        conversationText.text.fontSize = fontSize;
        conversationText.text.color = color;
        conversationText.go.transform.position = Camera.main.WorldToScreenPoint(position);
        conversationText.motion = motion;

        conversationText.speaker.text = speaker;
        conversationText.speaker.fontSize = fontSize;
        conversationText.speaker.color = color;
        conversationText.goSpeaker.transform.position = Camera.main.WorldToScreenPoint(position + new Vector3(0,0.2f,0));

        conversationText.MostraTesto();
    }

    // Getter di un ConversationText se non esiste lo crea
    public ConversationText GetConversationText(){

        if (conversationText==null){
            conversationText = new ConversationText();
            
            conversationText.go = Instantiate(textPrefab);
            conversationText.go.transform.SetParent(textContainer.transform);           
            conversationText.text = conversationText.go.GetComponent<Text>();
            
            conversationText.goSpeaker = Instantiate(textPrefab);
            conversationText.goSpeaker.transform.SetParent(conversationText.go.transform);           
            conversationText.speaker = conversationText.goSpeaker.GetComponent<Text>();
            
            conversationText.sfondo = textContainer.GetComponent<Image>();
        }

        return conversationText;
    }

    // Se esiste un ConversationText lo aggiorno. Uso LateUpdate perchè se dovesse capitare che si aggiorna prima di altro mi ritrovo che la barra spaziatrice che chiude il dialogo, lo riapre da capo.
    private void LateUpdate(){
        if (conversationText!=null){
            conversationText.UpdateConversationText();
        } 
    }
}
