using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutSceneIniziale : MonoBehaviour
{
    public bool fired = false;
    public bool finisced = false;
    private string sceneName = "Windill";
    
    private Text nomeSpeaker;
    private Text testo;

    float atteso = 0;

    void Start(){
        fired=((GameManager.instanza.timeSystem.giorno-1) + (GameManager.instanza.timeSystem.ora-8) + GameManager.instanza.timeSystem.minuto)>0;
        if (!fired)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!fired)
        {
            fired=((GameManager.instanza.timeSystem.giorno-1) + (GameManager.instanza.timeSystem.ora-8) + GameManager.instanza.timeSystem.minuto)>0;
        
            if (SceneManager.GetActiveScene().name!=sceneName)
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                StartCoroutine(PerformCutscene());
            }
        }

        if (finisced)
        {
            GameManager.instanza.cutScene=false;
            SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
            Destroy(this);
        }
        
    }

    IEnumerator PerformCutscene()
    {

        fired=true;
        
        GameManager.instanza.cutScene=true;

        this.gameObject.name=this.gameObject.name + "DaDistruggere";
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetSceneByName(sceneName));
            
        GameManager.instanza.player.gameObject.SetActive(false);

        TimelineController timelineController = GameObject.Find("TimelineController").GetComponent<TimelineController>();

        timelineController.playableDirectors[0].Play();
        
        yield return new WaitForSeconds(5);
        atteso= atteso + 5;
        yield return new WaitForSeconds(230.0f/60+0.1f);
        atteso= atteso + 230.0f/60 +0.1f;

        GameObject conversationTextManager = GameObject.Find("CanvasScena/ConversationTextManager");
 
        nomeSpeaker=conversationTextManager.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        testo=conversationTextManager.transform.GetChild(0).GetComponent<Text>();

        yield return Conversazione("Tomoe", "Nemaco!", 1.5f);
        yield return Conversazione("Tomoe", "...", 1.5f);
        yield return Conversazione("Tomoe", "NEMACO SVEGLIATI!", 1.5f);
        yield return Conversazione("Tomoe", "...", 1.5f);
        yield return Conversazione("Tomoe", "NEMACOOOOOOOO", 2.5f);
        
        yield return Conversazione("Nemaco", "Buongiorno Tomo...", (1118.0f-1040.0f)/60 + 1.0f);
        
        yield return Conversazione("Tomoe", "Era ora fannullone, muoviti che il capovillaggio ti aspetta", 2.0f);
        
        yield return Conversazione("Nemaco", "Ma sicura fosse oggi? Non posso dormire altri 5 minuti?", 2.0f);
        
        yield return Conversazione("Tomoe", "GRRRRRR", 1.0f);
        
        yield return Conversazione("Nemaco", "Giusto, corriamo dal capovillaggio", 1.5f);

        //yield return new WaitForSeconds((float)timelineController.playableDirectors[0].duration - 0.1f - atteso);

        GameObject nemaco = GameObject.Find("NemacoScena");
        GameManager.instanza.player.gameObject.SetActive(true);
        GameManager.instanza.player.transform.position=nemaco.transform.position;

        GameObject tomoe = GameObject.Find("TomoeScena");
        GameObject.Find("Alleato").gameObject.SetActive(true);
        GameObject.Find("Alleato").gameObject.transform.position=tomoe.transform.position;

        yield return new WaitForSeconds((float)timelineController.playableDirectors[0].duration - atteso);

        //yield return new WaitForSeconds(36.0f/60);

        /*
        Destroy(nemaco);
        Debug.Log("distruggo l'attore");
        Destroy(GameObject.Find("CanvasScena"));
        Debug.Log("distruggo il canvas");
        Destroy(GameObject.Find("CameraScena"));
        Debug.Log("distruggo la camera");
        */

        finisced=true;
        GameManager.instanza.cutScene=false;
        
        Destroy(this.gameObject);
    }

    
    WaitForSeconds Conversazione(string nome, string frase, float durata){
        nomeSpeaker.text=nome;
        testo.text=frase;
        
        atteso= atteso + durata;
        return new WaitForSeconds(durata);
    }
}
