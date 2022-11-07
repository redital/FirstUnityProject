using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class PausableMonoBehaviour : MonoBehaviour,IObservable
{
    private bool paused = false;
    
    private void Awake()
    {
        this.StartListening();
    }

    private void OnDestroy()
    {
        this.StopListening();
    }

    public void StartListening()
    {
        this.AddObserver(ObserverEventNames.PAUSEGAME);
        this.AddObserver(ObserverEventNames.RESUMEGAME);
    }

    public void StopListening()
    {
        this.RemoveObserver(ObserverEventNames.PAUSEGAME);
        this.RemoveObserver(ObserverEventNames.RESUMEGAME);
    }

    public void OnEvent(string eventName, object objectPassed)
    {
        switch (eventName)
        {
            case ObserverEventNames.PAUSEGAME:
                this.paused = true;
                break;
            case ObserverEventNames.RESUMEGAME:
                this.paused = false;
                break;
        }
    }

    private void Update()
    {
        if (!this.paused)
        {
            this.PausableUpdate();
        }
        
        this.NotPausableUpdate();
    }

    protected abstract void PausableUpdate();

    protected abstract void NotPausableUpdate();
}
