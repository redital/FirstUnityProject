using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using static Gestione.GestioneDizionari;    //Penso possa essere rimosso e fare tutto in maniera molto più semplice

public class GameManager : MonoBehaviour
{
    public static GameManager instanza;

    /*
    Deve sempre esserci una ed una sola instanza di tipo GameManager
    */
    private void Awake(){
        if(instanza==null){
            instanza=this;
        }
        else {
            Destroy (gameObject);
            return;
        }
        SceneManager.sceneLoaded += RiposizionaGiocatore;   // Ogni volta che viene caricata una nuova scena va posizionato il giocatore nel suo SpownPoint
        SceneManager.sceneLoaded += Pulizia;                // Ogni volta che viene caricata una nuova scena occorre eliminare eventuali duplicati di alcuni oggetti unici (quelli che ci si porta nel DontDestroyOnLoad)
        SceneManager.sceneLoaded += Carica; // Gestione tramite eventi: questa riga indica che quando accade SceneManager.sceneLoaded allora deve seguire Carica
        DontDestroyOnLoad(gameObject);

        // Non vogliamo distruggere oggetti di valenza generale tra una scena e l'altra
        DontDestroyOnLoad(instanza.player.gameObject);
        DontDestroyOnLoad(instanza.floatingTextManager.transform.parent.gameObject);
        DontDestroyOnLoad(instanza.barraPA.transform.parent.gameObject);                // Si potrebbe cambiare direttamente con il padre (HUD)
        DontDestroyOnLoad(instanza.menuDiPausa.gameObject);
    }

    // Risorse
    // Tutte le informazioni sono salvate all'interno di file che vengono letti quando i dati vengono caricati e memorizzati all'interno di dizionari
    public Dictionary<string, string> stats = new Dictionary<string, string>();
    public Dictionary<string, string> inventario = new Dictionary<string, string>();
    // Il salvataggio/Caricamento della posizione non è ancora stato implementato, ci sono stati dei tentativi fallimentari :(
    public Dictionary<string, string> posizione = new Dictionary<string, string>();
    
    // Queste variabili non servono davvero sono state messe per comodità di testing, sono solo copie dei dati salvati nei dizionari che dichiarate pubbliche appaiono nell'inspector, così posso vederle cambiare in tempo reale
    public int monete;
    public string scena;
    public Vector3 position;

    // Riferimenti alla UI
    public MenuDiPausa menuDiPausa;
    public BarraPA barraPA;             // Si potrebbe cambiare direttamente con il padre (HUD) (è quello di prima)

    // Riferimenti ai TextManager
    public FloatingTextManager floatingTextManager;
    public ConversationTextManager conversationTextManager;

    // Riferimento al giocatore
    public Player player;

    // Indicatore del se si sta combattendo o meno (da implementare)
    public List<Enemy> chasingEnemy = new List<Enemy>();
    public bool combatStatus;

    
    
    // Logica

    /* Metodo per mostrare sullo schermo un FloatingText
        Parametri:
        * testo (obbligatorio):     testo da mostrare
        * position (obbligatorio):  posizione in cui mostrare il testo, si fa riferimento al centro del testo. Per la maggior parte degli utilizzi si usa transorm.position o una sua lieve traslazione
        * motion:                   vettore di movimento del testo (Default: new Vector3(0,0,0) cioè non si muove)
        * color:                    colore della scritta (Default: bianco)
        * fontSize                  dimensioni dei caratteri del testo (Default: 33)
        * durata                    dopo quanto tempo deve scomparire il testo

        Questo metodo richiama il metodo del FloatingTextManager e serve per impostare i valori di default ma sopratutto per permettere una gestione centralizzata
    */
    public void MostraFloatingText(string testo, Vector3 position, Vector3? motion = null, Color? color = null, int fontSize = 33, float durata = 1.5f){
        if(color==null){
            color=Color.white;
        }
        if(motion==null){
            motion = new Vector3(0,0,0);
        }
        
        floatingTextManager.MostraTesto(testo,  fontSize,  ((Color)color),  ((Vector3)position),  ((Vector3)motion),  durata);
    }
    
    /* Metodo per mostrare sullo schermo un ConversationText
        Parametri:
        * testo (obbligatorio):     testo da mostrare
        * speaker (obbligatorio):   nome del personaggio che parla
        * position:                 posizione in cui mostrare il testo, si fa riferimento all'angolo in alto a sinistra del testo. Per la maggior 
                                    parte degli utilizzi lasciare il Defaul, è da vedere se ha senso lasciarlo come parametro (Default: lato sinistro dello schermo, un terzo dell'altezza)
        * motion:                   vettore di movimento del testo (Default: new Vector3(0,0,0) cioè non si muove)
        * color:                    colore della scritta (Default: bianco)
        * fontSize                  dimensioni dei caratteri del testo (Default: 33)

        Questo metodo richiama il metodo del ConversationTextManager e serve per impostare i valori di default ma sopratutto per permettere una gestione centralizzata
    */
    public void MostraConversationText(string[] testo, string speaker, Vector3? position = null, Vector3? motion = null, Color? color = null, int fontSize = 33){
        if(conversationTextManager.GetConversationText().attivo){
            return;
        }


        if(color==null){
            color=Color.white;
        }
        if(position==null){
            position = Camera.main.ViewportToWorldPoint (new Vector3(390.0f/Camera.main.pixelWidth + 0.01f, 0.20f,0));
            //Debug.Log(300.0f/Camera.main.pixelWidth);
        }
        if(motion==null){
            motion = new Vector3(0,0,0);
        }
        
        conversationTextManager.MostraTesto(testo, speaker,  fontSize,  ((Color)color),  ((Vector3)position),  ((Vector3)motion));
    }

    public void RiposizionaGiocatore(Scene S, LoadSceneMode mode){
        try
        {
            player.transform.position = GameObject.Find("SpawnPoint").transform.position;
        }
        catch (System.Exception)
        {
        }
    }

    public void Pulizia(Scene S, LoadSceneMode mode){
        DistruggiDuplicati(instanza.player.gameObject);
        DistruggiDuplicati(instanza.floatingTextManager.transform.parent.gameObject);
        DistruggiDuplicati(instanza.barraPA.transform.parent.gameObject);               // Si potrebbe cambiare direttamente con il padre (HUD) (è quello di prima)
        DistruggiDuplicati(instanza.menuDiPausa.gameObject);
    }

    public void DistruggiDuplicati(GameObject oggetto){
        GameObject[] temp = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];   // Prendo tutti i GameObject
        List<GameObject> gos = new List<GameObject>();
        for (int i = 0; i < temp.Length; i++){          // Tra tutti i GameObject cerco quelli che hanno lo stesso nome di quelli d'interesse
            if (temp[i].name==oggetto.name){
                gos.Add(temp[i]);
            }
        }
        for (int i = 0; i < gos.Count; i++){            // Tra tutti quelli con lo stesso nome tengo solo quello d'interesse e distruggo gli altri
            if (gos[i]!=oggetto)
            {
                Destroy(gos[i]);
            }
        }
    }

    //Aggiorna i file con i valori attualmente salvati nei dizionari
    public void Salva(){

        Gestione.GestioneDizionari.ScritturaDizionario(stats,"Statistiche.txt");
        Gestione.GestioneDizionari.ScritturaDizionario(inventario,"Inventario.txt");

        
        posizione["Scena"] = SceneManager.GetActiveScene().name;
        posizione["x"] = player.transform.position.x.ToString();
        posizione["y"] = player.transform.position.y.ToString();
        posizione["z"] = player.transform.position.z.ToString();
        Gestione.GestioneDizionari.ScritturaDizionario(posizione,"Posizione.txt");
        

        Debug.Log("Dati salvati");
    }

    //Aggiorna i dizionari con i valori attualmente salvati nei file
    public void Carica(){
        stats = Gestione.GestioneDizionari.LetturaDizionario("Statistiche.txt");
        inventario = Gestione.GestioneDizionari.LetturaDizionario("Inventario.txt");
        monete = int.Parse(inventario["Monete"]);
        player.LoadStats();

        
        posizione = Gestione.GestioneDizionari.LetturaDizionario("Posizione.txt");
        scena = posizione["Scena"];
        SceneManager.LoadScene(scena);
        player.transform.position = new Vector3(float.Parse(posizione["x"]),float.Parse(posizione["y"]),float.Parse(posizione["z"]));
        

        Debug.Log("Dati caricati");
    }

    //Versione del metodo Carica con questi input che non ho ben capito per far caricare i dati appena parte il gioco 
    public void Carica(Scene S, LoadSceneMode mode){
        Carica();
        SceneManager.sceneLoaded -= Carica; //Commentando questa riga il caricamento viene eseguito ogni volta che si cambia scena
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.L)){
            Carica();
        }

        if (Input.GetKeyDown(KeyCode.J)){
            Salva();
        }

        if (Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene("MainMenu");
            Time.timeScale=1;
        }

        if (chasingEnemy.Count!=0){
            combatStatus=true;
        }
        else{
            combatStatus=false;
        }
        
        //stampa di controllo, effettivamente potrebbe essere tolta ormai
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log(string.Join(", ", chasingEnemy));
        }

         /*
        Le funzioni Update (e le sue varianti) vengono richiamate ad ogni frame, gestiamo quindi qui tutti gli input.
        */
    }

}