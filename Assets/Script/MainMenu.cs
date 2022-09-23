using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    void Start(){
        if (GameManager.instanza==null)
        {
            return;
        }
        Destroy(GameManager.instanza.player.gameObject);
        Destroy(GameManager.instanza.floatingTextManager.transform.parent.gameObject);
        Destroy(GameManager.instanza.barraPA.transform.parent.gameObject);
        Destroy(GameManager.instanza.menuDiPausa.gameObject);

        Destroy(GameManager.instanza.gameObject);
        //SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetSceneByName("MainMenu"));
    }

    //Metodi creati per essere richiamati dai pulsanti del MainMenù
    public void PlayGame(){
        SceneManager.LoadScene("Main");
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void NewGame(int slotScelto){
        string percorso="Assets/Salvataggi";
        //int slotScelto = 2;
        
        File.Copy(percorso + "/Default" +  "/Statistiche.txt", percorso + "/Slot" + slotScelto +  "/Statistiche.txt",true);
        File.Copy(percorso + "/Default" +  "/Inventario.txt", percorso + "/Slot" + slotScelto +  "/Inventario.txt",true);
        File.Copy(percorso + "/Default" +  "/PosizioniInventario.txt", percorso + "/Slot" + slotScelto +  "/PosizioniInventario.txt",true);
        File.Copy(percorso + "/Default" +  "/SkillApprese.txt", percorso + "/Slot" + slotScelto +  "/SkillApprese.txt",true);
        File.Copy(percorso + "/Default" +  "/SkillEquipaggiate.txt", percorso + "/Slot" + slotScelto +  "/SkillEquipaggiate.txt",true);
        File.Copy(percorso + "/Default" +  "/Posizione.txt", percorso + "/Slot" + slotScelto +  "/Posizione.txt",true);

        using (StreamWriter outputFile = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "Assets/Salvataggi/UltimoSlot.txt"))){
            outputFile.WriteLine(slotScelto.ToString());
        }

        PlayGame();
    }

    public void LoadGame(int slotScelto){
        //int slotScelto = 2;
       
        using (StreamWriter outputFile = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "Assets/Salvataggi/UltimoSlot.txt"))){
            outputFile.WriteLine(slotScelto.ToString());
        }

        PlayGame();
    }

    void Update(){
        
        if (Input.GetKeyDown(KeyCode.N)){
            NewGame(2);
        }
    }
}
