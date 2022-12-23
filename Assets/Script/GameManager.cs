using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.IO;

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
        SceneManager.sceneLoaded += CreaEventSystem;
        SceneManager.sceneLoaded += RiposizionaGiocatore;   // Ogni volta che viene caricata una nuova scena va posizionato il giocatore nel suo SpownPoint
        SceneManager.sceneLoaded += Pulizia;                // Ogni volta che viene caricata una nuova scena occorre eliminare eventuali duplicati di alcuni oggetti unici (quelli che ci si porta nel DontDestroyOnLoad)
        SceneManager.sceneLoaded += Carica;                 // Gestione tramite eventi: questa riga indica che quando accade SceneManager.sceneLoaded allora deve seguire Carica
        DontDestroyOnLoad(gameObject);

        // Non vogliamo distruggere oggetti di valenza generale tra una scena e l'altra
        DontDestroyOnLoad(instanza.player.gameObject);
        DontDestroyOnLoad(instanza.floatingTextManager.transform.parent.gameObject);
        DontDestroyOnLoad(instanza.barraPA.transform.parent.gameObject);                // Si potrebbe cambiare direttamente con il padre (HUD)
        DontDestroyOnLoad(instanza.menuDiPausa.gameObject);
        //DontDestroyOnLoad(instanza.eventSystem);

        // Volevo metterlo nello start ma siccome il primo Carica viene chiamato a scena caricata nell'Awake, alloraavverrebbe dopo, ma queste cose servono per il carica quindi lo metto qui
        skillList=LetturaListaSkill(percorsoSalvataggio + slotSalvataggio + "/" + "ListaSkill.txt");
        itemList=LetturaListaItems(percorsoSalvataggio + slotSalvataggio + "/" + "ListaOggetti.txt");

        
        using (var sr = new StreamReader("Assets/Salvataggi/UltimoSlot.txt"))
        {
            slotSalvataggio=int.Parse(sr.ReadLine());
        }
    }

    // Risorse

    // Tutte le informazioni sono salvate all'interno di file che vengono letti quando i dati vengono caricati e memorizzati all'interno di dizionari
    public Dictionary<string, string> stats = new Dictionary<string, string>();
    public Dictionary<int, Item> inventario = new Dictionary<int, Item>();
    public Dictionary<string, string> posizioniInventario = new Dictionary<string, string>();
    public Dictionary<string, string> posizione = new Dictionary<string, string>();

    private string percorsoSalvataggio = "Assets/Salvataggi/Slot";
    public int slotSalvataggio=1;
    
    /*
    // Queste variabili non servono davvero sono state messe per comodità di testing, sono solo copie dei dati salvati nei dizionari che dichiarate pubbliche appaiono nell'inspector, così posso vederle cambiare in tempo reale
    public int monete;
    public string scena;
    public Vector3 position;
    */

    // Riferimenti alla UI
    public MenuDiPausa menuDiPausa;
    public GameObject eventSystem;
    public BarraPA barraPA;             // Si potrebbe cambiare direttamente con il padre (HUD) (è quello di prima)

    // Riferimenti ai TextManager
    public FloatingTextManager floatingTextManager;
    public ConversationTextManager conversationTextManager;

    // Riferimento al giocatore
    public Player player;

    // Riferimento al sistema di giorni e orari
    public TimeSystem timeSystem;
    
    // Gestione Quest Attive
    public GameObject quests;

    // Indicatore del se si sta combattendo o meno
    public List<Enemy> chasingEnemy = new List<Enemy>();
    public bool combatStatus;

    // Indicatore del se si sta parlando o meno
    public bool staParlando;

    // Indicatore del se il menù di pausa è aperto o meno
    public bool menuAperto;

    // Indicatore del se il caricamento dei dati è in corso o meno
    public bool caricando;

    // Indicatore del se il c'è una cutscene in corso o meno
    public bool cutScene;

    // Lista di tutte le skill
    public List<Skill> skillList = new List<Skill>();
    // Lista skill apprese
    public List<Skill> skillApprese = new List<Skill>();

    // Lista di tutti gli ogetti
    public List<Item> itemList = new List<Item>();



    
    
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
    public void MostraFloatingText(string testo, Vector3 position, Vector3? motion = null, Color? color = null, int fontSize = 46, float durata = 1.5f){
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
    public void MostraConversationText(string[] testo, string speaker, Vector3? position = null, Vector3? motion = null, Color? color = null, int fontSize = 46){
        if(staParlando){
            return;
        }


        if(color==null){
            color=Color.white;
        }
        if(position==null){
            position = Camera.main.ViewportToWorldPoint (new Vector3(900.0f/Camera.main.pixelWidth + 0.05f, 0.135f - 0.01f,0));
            //Debug.Log(300.0f/Camera.main.pixelWidth);
        }
        if(motion==null){
            motion = new Vector3(0,0,0);
        }
        
        conversationTextManager.MostraTesto(testo, speaker,  fontSize,  ((Color)color),  ((Vector3)position),  ((Vector3)motion));
    }

    public void CreaEventSystem(Scene S, LoadSceneMode mode){
        // Vorrei fare un ciclo ma per qualche motivo non cambia l'oggetto all'interno
        GameObject temp1=GameObject.Find("EventSystem");
        if (temp1!=null)
        {
            Destroy(temp1);
        }
        GameObject temp2=GameObject.Find("EventSystem(Clone)");
        if (temp2!=null)
        {
            Destroy(temp2);
        }
        temp1=GameObject.Find("EventSystem");
        if (temp1!=null)
        {
            Destroy(temp1);
        }
        temp2=GameObject.Find("EventSystem(Clone)");
        if (temp2!=null)
        {
            Destroy(temp2);
        }
        temp1=GameObject.Find("EventSystem");
        if (temp1!=null)
        {
            Destroy(temp1);
        }
        temp2=GameObject.Find("EventSystem(Clone)");
        if (temp2!=null)
        {
            Destroy(temp2);
        }
        /*
        GameObject temp2=GameObject.Find("EventSystem");
        if (temp2!=null)
        {
            Debug.Log(GameObject.Find("EventSystem"));
            Destroy(temp2);
        }
        GameObject temp3=GameObject.Find("EventSystem");
        if (temp3!=null)
        {
            Debug.Log(GameObject.Find("EventSystem"));
            Destroy(temp3);
        }
        */

        GameObject.Instantiate(eventSystem);
    }

    public void RiposizionaGiocatore(){
        try{
            player.transform.position = GameObject.Find("SpawnPoint").transform.position;
        }
        catch (System.Exception){
            Debug.Log("No SpawnPoint");
        }
    }

    public void RiposizionaGiocatore(Scene S, LoadSceneMode mode){
        if (!caricando)
        {
            RiposizionaGiocatore();
        }
        else{
            caricando=false;
        }
    }

    public void Pulizia(Scene S, LoadSceneMode mode){
        DistruggiDuplicati(instanza.player.gameObject);
        DistruggiDuplicati(instanza.floatingTextManager.transform.parent.gameObject);
        DistruggiDuplicati(instanza.barraPA.transform.parent.gameObject);               // Si potrebbe cambiare direttamente con il padre (HUD) (è quello di prima)
        DistruggiDuplicati(instanza.menuDiPausa.gameObject);
        //DistruggiDuplicati(instanza.eventSystem);
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

    public void FermaGioco(){
        Time.timeScale=0;
    }

    public void RiprendiGioco(){
        Time.timeScale=1;
    }

    //Aggiorna i file con i valori attualmente salvati nei dizionari
    public void Salva(){

        Gestione.GestioneDizionari.ScritturaDizionario(stats,percorsoSalvataggio + slotSalvataggio + "/" + "Statistiche.txt");
        Gestione.GestioneDizionari.ScritturaInventario(inventario,percorsoSalvataggio + slotSalvataggio + "/" + "Inventario.txt");
        Gestione.GestioneDizionari.ScritturaDizionario(posizioniInventario,percorsoSalvataggio + slotSalvataggio + "/" + "PosizioniInventario.txt");
        Gestione.GestioneDizionari.ScritturaSkillApprese(skillApprese,percorsoSalvataggio + slotSalvataggio + "/" + "SkillApprese.txt");
        Gestione.GestioneDizionari.ScritturaSkillEquipaggiate(player.skillSet,percorsoSalvataggio + slotSalvataggio + "/" + "SkillEquipaggiate.txt");

        
        posizione["Scena"] = SceneManager.GetActiveScene().name;
        posizione["x"] = player.transform.position.x.ToString();
        posizione["y"] = player.transform.position.y.ToString();
        posizione["z"] = player.transform.position.z.ToString();
        instanza.timeSystem.Salva();
        Gestione.GestioneDizionari.ScritturaDizionario(posizione,percorsoSalvataggio + slotSalvataggio + "/" + "Posizione.txt");
        

        Debug.Log("Dati salvati");
    }

    //Aggiorna i dizionari con i valori attualmente salvati nei file
    public void Carica(){
        caricando=true;
        
        stats = Gestione.GestioneDizionari.LetturaDizionario(percorsoSalvataggio + slotSalvataggio + "/" + "Statistiche.txt");
        inventario = Gestione.GestioneDizionari.LetturaInventario(percorsoSalvataggio + slotSalvataggio + "/" + "Inventario.txt");
        posizioniInventario = Gestione.GestioneDizionari.LetturaDizionario(percorsoSalvataggio + slotSalvataggio + "/" + "PosizioniInventario.txt");
        skillApprese = Gestione.GestioneDizionari.LetturaSkillApprese(percorsoSalvataggio + slotSalvataggio + "/" + "SkillApprese.txt");
        player.skillSet = Gestione.GestioneDizionari.LetturaSkillEquipaggiate(percorsoSalvataggio + slotSalvataggio + "/" + "SkillEquipaggiate.txt");
        
        //monete = int.Parse(stats["Monete"]);
        player.LoadStats();


        player.transform.GetChild(0).GetChild(0).GetComponent<Weapon>().nomeArma=itemList.Find(x => x.name==stats["Arma"]).name;
        player.transform.GetChild(0).GetChild(0).GetComponent<Weapon>().baseDamage=itemList.Find(x => x.name==stats["Arma"]).feature;
        player.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite=Resources.Load("SpriteArmi/"+itemList.Find(x => x.name==stats["Arma"]).spriteName.Split("_")[0] + "_sprite") as Sprite;


        
        posizione = Gestione.GestioneDizionari.LetturaDizionario(percorsoSalvataggio + slotSalvataggio + "/" + "Posizione.txt");
        //scena = posizione["Scena"];
        SceneManager.LoadScene(posizione["Scena"]);
        player.transform.position = new Vector3(float.Parse(posizione["x"]),float.Parse(posizione["y"]),float.Parse(posizione["z"]));
        instanza.timeSystem.Carica();
        

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
            instanza.menuDiPausa.NascondiMenuDiPausa();
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