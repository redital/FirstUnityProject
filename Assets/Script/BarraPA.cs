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
        barraPA.maxValue = int.Parse(GameManager.instanza.stats["PAMAX"]);
    }

    // Update is called once per frame
    void Update()
    {
        barraPA.value=GameObject.Find("Player").GetComponent<Player>().PA;
    }
}
