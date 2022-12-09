using System;
using UnityEngine;

public abstract class PausableMonoBehaviour : MonoBehaviour,IObservable
{
    private bool paused = false;

    private bool Paused
    {
        get
        {
            return this.paused;
        }
        set
        {
            paused = value;
        }
    }
    
    protected virtual void Awake()
    {
        this.StartListening();
    }

    protected virtual void OnDestroy()
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
                this.Paused = true;
                break;
            case ObserverEventNames.RESUMEGAME:
                this.Paused = false;
                break;
        }
    }

    private void Update()
    {
        if (!this.paused)
        {
            this.PausableUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (!this.paused)
        {
            this.PausableFixedUpdate();
        }
    }

    protected abstract void PausableUpdate();
    protected abstract void PausableFixedUpdate();
}
