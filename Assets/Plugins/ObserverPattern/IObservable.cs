using System;
using System.Collections;
using UnityEngine;

public interface IObservable
{
    void StartListening();

    void StopListening();

    void OnEvent(string eventName,object objectPassed);

    //IEnumerator OnEventAsync(string eventName, object objectPassed);
}