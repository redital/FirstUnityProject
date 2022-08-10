using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraPA : MonoBehaviour
{
    public Slider barraPA;

    // Start is called before the first frame update
    void Start()
    {
        barraPA.maxValue = GameManager.instanza.player.PAMAX;
    }

    // Update is called once per frame
    void Update()
    {
        barraPA.value=GameManager.instanza.player.PA;
    }
}
