using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor.VersionControl;

public class Talker : Collidable
{

    /* Classe creata per parlare con qualsiasi entità parlante 
        Questo script va legato ad un GameObject figlio dell'oggetto parlante
    */

    private string nome = "NPC a caso";                                                      // Nome dell'oggetto parlante
    [SerializeField] 
    private string[] frasi = new string[] {"Ah ma sei tu!", "Adesso mi tocca incularti"};    // Frasi che dirà (Si pensa di cambiare in una lista di vettori di stringhe per dare varietà)

    [SerializeField] 
    private TextAsset fileDialogo;
    [SerializeField] 
    private bool testoDaFile;

    // Start is called before the first frame update
    protected override void Start()
    {
        boxCollider=transform.GetComponent<BoxCollider2D>(); 
        nome = transform.name;
        if (testoDaFile)
        {
            frasi=parsingConversetion(fileDialogo);
        }
        
    }

    protected string[] parsingConversetion(TextAsset dialogo){
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
    
    protected override void OnCollide(Collider2D coll){
        if (Input.GetKeyDown(KeyCode.Space) & !GameManager.instanza.combatStatus & !GameManager.instanza.menuAperto){
            if (coll.name == GameManager.instanza.player.name){
                GameManager.instanza.MostraConversationText(frasi,nome);
            } 
        }
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (GetComponent<NPC>()!=null){
            GetComponent<NPC>().wanderingMoveSpeed=0.16f;
            if ((transform.position-GameManager.instanza.player.transform.position).x<0.32f & (transform.position-GameManager.instanza.player.transform.position).y<0.32f){
                NPC mover=GetComponent<NPC>();
                mover.wanderingMoveSpeed=0;
                if (Math.Abs((transform.position-GameManager.instanza.player.transform.position).x)>Math.Abs((transform.position-GameManager.instanza.player.transform.position).y)){
                    if((transform.position-GameManager.instanza.player.transform.position).x<0){
                        GetComponent<SpriteRenderer>().sprite = mover.diLato;
                        transform.localScale = new Vector3(1,1,1);
                    }
                    if((transform.position-GameManager.instanza.player.transform.position).x>0){
                        GetComponent<SpriteRenderer>().sprite = mover.diLato; 
                        transform.localScale = new Vector3(-1,1,1);
                    }
                }
                else{
                    if((transform.position-GameManager.instanza.player.transform.position).y<0){
                        GetComponent<SpriteRenderer>().sprite = mover.dietro;
                    }
                    if((transform.position-GameManager.instanza.player.transform.position).y>0){
                        GetComponent<SpriteRenderer>().sprite = mover.davanti;
                    }
                }            
            }
        }
    }
}
