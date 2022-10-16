using System.Collections.Generic;
using Debug = UnityEngine.Debug;

/// <summary>
/// COME FUNZIONA:
///
/// 1) Si aggiunge l'evento a ObserverEventNames
/// 2) Si triggera l'evento tramite ObserverEventManager.Trigger<"tipopassato>(nomeEvento,eventuale roba da passare) NOTA: se si vuole passare null, ObserverEventManager.Trigger<"object>(nomeEvento,null)
/// 3) Chi deve mettersi in ascolto dell'evento implementa l'interfaccia IObservable
/// 4) In StartListening, in Start o in Awake si aggiunge ObserverEventManager.AddObserver(nomeEvento,this)
/// 5) Si aggiunge StartListening() in Start()
/// 6) Procedimento simile va fatto in StopListening che poi va aggiunto in OnDestroy
/// 7) In OnEvent se il nome dell'evento è quello desiderato si fa quel che si deve
///
/// </summary>
public static class ObserverEventManager
{
    private static Dictionary<string, List<IObservable>> _observers = new Dictionary<string, List<IObservable>>();

    public static void AddObserver(string eventName, IObservable observer)
    {
        if (_observers.ContainsKey(eventName))
        {
            if (_observers[eventName].Contains(observer))
            {
                Debug.Log("Observer already added");
                return;
            }

            _observers[eventName].Add(observer);
        }
        else
        {
            _observers.Add(eventName, new List<IObservable> { observer });
        }
    }
    
    public static void AddObserver(this IObservable observer,string eventName)
    {
        AddObserver(eventName,observer);
    }

    public static void RemoveObserver(this IObservable observer, string eventName)
    {
        RemoveObserver(eventName, observer);
    }

    public static void RemoveObserver(string eventName, IObservable observer)
    {
        if (_observers.ContainsKey(eventName))
        {
            if (_observers[eventName].Contains(observer))
            {
                _observers[eventName].Remove(observer);
            }
        }
    }

    public static void Trigger<T>(string eventName, T objectToPass)
    {
        Debug.Log($"Evento triggerato {eventName}");

        // Prevenire la modifica dell'enumeratore durante l'iterazione
        
        //Debug.Log(new StackTrace().GetFrame(1).GetMethod().Name);

        Dictionary<string, List<IObservable>> observersTemp = GetTemporaryObservers();

        foreach (var kv in observersTemp)
        {
            if (kv.Key != eventName) continue;

            foreach (var observer in kv.Value)
            {
                if (observer == null) continue;

                observer.OnEvent(eventName,objectToPass);
            }
        }

        observersTemp.Clear();
    }

    private static Dictionary<string, List<IObservable>> GetTemporaryObservers()
    {
        Dictionary<string, List<IObservable>> observersTemp = new Dictionary<string, List<IObservable>>();

        foreach (var kv in _observers)
        {
            observersTemp.Add(kv.Key, kv.Value);
        }

        return observersTemp;
    }
}