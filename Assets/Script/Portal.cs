using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Collidable
{
    public string[] sceneNames;     //Nomi delle scene, ne verrà caricata una a caso (probabilmente da cambiare a seconda del GamePlay)

    protected override void OnCollide(Collider2D coll){
       if (coll.name == "Player"){
            string sceneName=sceneNames[Random.Range(0,sceneNames.Length)];
            SceneManager.LoadScene(sceneName);
        } 
    }
    
}
