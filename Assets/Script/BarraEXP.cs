using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraEXP : MonoBehaviour
{
    public Slider barraEXP;

    // Start is called before the first frame update
    void Start()
    {
        barraEXP.maxValue = GameManager.instanza.player.NextLevelEXP;
    }

    // Update is called once per frame
    void Update()
    {
        barraEXP.value = GameManager.instanza.player.EXP;
        barraEXP.maxValue = GameManager.instanza.player.NextLevelEXP;
    }
}
