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
    [SerializeField] 
    private bool testoComplesso;
    private bool finito=true;
    private int arrivatoA=0;

    private bool isQuestGiver=false;

    // Start is called before the first frame update
    protected override void Start()
    {
        boxCollider=transform.GetComponent<BoxCollider2D>(); 
        nome = transform.name;
        if (testoDaFile)
        {
            frasi=parsingConversetion(fileDialogo);
        }
        
        if (transform.GetComponent<QuestGiver>()!=null)
        {
            isQuestGiver=true;
        }
    }

    protected string[] parsingConversetion(TextAsset dialogo){
        ArrayList testo = new ArrayList();
        return parsingConversetion(dialogo.text);
    }

    protected string[] parsingConversetion(string dialogo){
        ArrayList testo = new ArrayList();
        string dialogoText=dialogo.Replace("\n", "").Replace("\r", ""); // rimuovo i ritorno a capo
        string startTr1 = "<battuta>"; 
        string endTr1 = "</battuta>";

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

    protected void performComplexConversetion(TextAsset dialogo){
        finito=false;

        string dialogoText=dialogo.text.Replace("\n", "").Replace("\r", ""); // rimuovo i ritorno a capo
        string start = "<obj>"; 
        string end = "</obj>";
        string startTr1 = "<talker>"; 
        string endTr1 = "</talker>";

        //Debug.Log(dialogoText);
        dialogoText=dialogoText.Remove(0,arrivatoA);
        /*
        while (dialogoText.Length!= 0)
        {
            int inizioNome = dialogoText.IndexOf(startTr1,0)+startTr1.Length;
            int fineNome = dialogoText.IndexOf(endTr1,inizioNome);

            string nome = dialogoText.Substring(inizioNome,fineNome-inizioNome);

            int inizio=dialogoText.IndexOf(start,0)+start.Length;
            Debug.Log("inizio "+inizio);
            int fine=dialogoText.IndexOf(end,inizio);
            Debug.Log("fine "+fine);

            Debug.Log(dialogoText.Substring(fineNome + endTr1.Length,fine-(fineNome + endTr1.Length)));
            GameManager.instanza.MostraConversationText(parsingConversetion(dialogoText.Substring(fineNome + endTr1.Length,fine - (fineNome + endTr1.Length))),nome);

            arrivatoA = fine + end.Length;

            //parlanti.Add(dialogoText.Substring(inizio,fine-inizio));
            dialogoText=dialogoText.Remove(inizio-start.Length, fine-inizio+end.Length+start.Length);
            //Debug.Log(dialogoText);
        }
        */
        int inizioNome = dialogoText.IndexOf(startTr1,0)+startTr1.Length;
        int fineNome = dialogoText.IndexOf(endTr1,inizioNome);

        string nome = dialogoText.Substring(inizioNome,fineNome-inizioNome);

        int inizio=dialogoText.IndexOf(start,0)+start.Length;
        //Debug.Log("inizio "+inizio);
        int fine=dialogoText.IndexOf(end,inizio);
        //Debug.Log("fine "+fine);

        Debug.Log(dialogoText.Substring(fineNome + endTr1.Length,fine-(fineNome + endTr1.Length)));
        GameManager.instanza.MostraConversationText(parsingConversetion(dialogoText.Substring(fineNome + endTr1.Length,fine - (fineNome + endTr1.Length))),nome);

        arrivatoA += fine + end.Length;
        Debug.Log(arrivatoA);
        Debug.Log(fileDialogo.text.Replace("\n", "").Replace("\r", "").Length);
    }
    
    
    protected override void OnCollide(Collider2D coll){
        if (Input.GetKeyDown(KeyCode.Space) & finito & !GameManager.instanza.combatStatus & !GameManager.instanza.menuAperto){
            if (coll.name == GameManager.instanza.player.name){
                Interagisci();
            } 
        }
    }

    protected void Interagisci(){
        if (isQuestGiver)
        {
            GestistioneStatoQuest();
        }
        else if (testoComplesso)
        {
            performComplexConversetion(fileDialogo);
        }
        else
        {
            GameManager.instanza.MostraConversationText(frasi,nome);
        }
    }

    protected void GestistioneStatoQuest()
    {
        QuestGiver questGiver = transform.GetComponent<QuestGiver>();
        if (!questGiver.AssignedQuest && !questGiver.Helped)
        {
            questGiver.AssignQuest();
            GameManager.instanza.MostraConversationText(questGiver.GetFrasiAssegnazione(),nome);
        }
        else if(questGiver.AssignedQuest && !questGiver.Helped)
        {
            if(questGiver.CheckQuest()){
                GameManager.instanza.MostraConversationText(questGiver.GetFrasiConclusione(),nome);
            }
            else{
                GameManager.instanza.MostraConversationText(questGiver.GetFrasiIncompleta(),nome);
            }
        }
        else
        {
            GameManager.instanza.MostraConversationText(questGiver.GetFrasiCompleta(),nome);
            //DialogueSystem.Instance.AddNewDialogue(new string[] { "Thanks for that stuff that one time." }, name);
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

        if (!GameManager.instanza.staParlando & !finito)
        {                
            Debug.Log(arrivatoA);
            Debug.Log(fileDialogo.text.Replace("\n", "").Replace("\r", "").Length);

            if (arrivatoA==fileDialogo.text.Replace("\n", "").Replace("\r", "").Length)
            {
                finito=true;
                arrivatoA=0;
            }
            else
            {
                performComplexConversetion(fileDialogo);
            }
        }
    }
}
