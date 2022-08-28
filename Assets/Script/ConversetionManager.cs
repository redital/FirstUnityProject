// questo script serve a leggere la conversazione da un file di testo e mostrarla

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor.VersionControl;

public class ConversetionManager : MonoBehaviour
{
  
     private GameObject player;
     [SerializeField] 
     public TextAsset dialogo;
     private string[] testo;

     //parsing del testo da visualizare
     public string[] parsingConversetion(){
          ArrayList testo = new ArrayList();
          string dialogoText=dialogo.text.Replace("\n", "").Replace("\r", ""); // rimuovo i ritorno a capo
          string startTr1 = "<obj>"; 
          string endTr1 = "</obj>";

          //Debug.Log(dialogoText);
          while (dialogoText.Length!= 0)
          {
               int inizio=dialogoText.IndexOf(startTr1,0)+startTr1.Length;
               //Debug.Log("inizio "+inizio);
               int fine= dialogoText.IndexOf(endTr1,inizio);
               //Debug.Log("fine "+fine);

               testo.Add(dialogoText.Substring(inizio,fine-inizio));
               dialogoText=dialogoText.Remove(inizio-startTr1.Length, fine-inizio+endTr1.Length+startTr1.Length);
               //Debug.Log(dialogoText);
          }
               string[] text = (String[]) testo.ToArray(typeof(string));
               return text;
     }

     void Start(){
          player =  GameManager.instanza.player.gameObject;
          testo = parsingConversetion();
          /*   foreach (var a in testo)
          {
               Debug.Log("in start: "+a);
          } */
     }

     //funzione che controlla la collisione
     bool checkCollision(Collider2D coll){
        BoxCollider2D mainColl=GetComponent<BoxCollider2D>();
        float distanza = mainColl.Distance(coll).distance;
        return distanza<=0 ? true : false;
     }
    
    
     void Update() {
          bool coll = false;
     
          coll=checkCollision(player.GetComponent<BoxCollider2D>());
          if(coll & Input.GetKeyDown(KeyCode.Space)){
               GameManager.instanza.MostraConversationText(testo,name);
          }
          //Debug.Log(name);
          //Debug.Log(coll);
     }
}
