using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineController : MonoBehaviour {

    public List<PlayableDirector> playableDirectors;


    public void Play()
    {
        foreach (PlayableDirector playableDirector in playableDirectors) 
        {
            playableDirector.Play ();
        }
    }

    void Update(){

        GameManager.instanza.barraPA.transform.parent.GetComponent<Canvas>().enabled=!GameManager.instanza.cutScene;

    }
}