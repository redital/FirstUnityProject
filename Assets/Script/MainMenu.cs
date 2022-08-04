using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Metodi creati per essere richiamati dai pulsanti del MainMenù
    public void PlayGame(){
        SceneManager.LoadScene("Main");
    }

    public void QuitGame(){
        Application.Quit();
    }
}
