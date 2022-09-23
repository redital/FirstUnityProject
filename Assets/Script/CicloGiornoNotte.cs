using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CicloGiornoNotte : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.Rendering.Universal.Light2D luce;
    [SerializeField]
    private Gradient colore;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        luce.color = colore.Evaluate((float)((GameManager.instanza.timeSystem.ora-8)*60 + GameManager.instanza.timeSystem.minuto)/(16*60));
    }
}
