using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraPV : MonoBehaviour
{
    public Slider barraPV;

    // Start is called before the first frame update
    void Start()
    {
        barraPV.maxValue = GameManager.instanza.player.PVMAX;;
    }

    // Update is called once per frame
    void Update()
    {
        barraPV.value=GameManager.instanza.player.PV;

    }
}
