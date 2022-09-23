using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    public int giorno, ora, minuto;
    float ultimoScatto=0;
    float durataMinuto=3.75f;
    [SerializeField] 
    private TMPro.TextMeshProUGUI testoOrario;

    // Start is called before the first frame update
    void Start()
    {
        Carica();
        Stampa();
    }

    public void Carica(){
        giorno=int.Parse(GameManager.instanza.posizione["Giorno"]);
        ora=int.Parse(GameManager.instanza.posizione["Ora"]);
        minuto=int.Parse(GameManager.instanza.posizione["Minuto"]);
    }

    public void Salva(){
        GameManager.instanza.posizione["Giorno"]=giorno.ToString();
        GameManager.instanza.posizione["Ora"]=ora.ToString();
        GameManager.instanza.posizione["Minuto"]=minuto.ToString();
    }

    public void Stampa(){
        if (minuto<10)
        {
            testoOrario.text= "Giorno " + giorno + "\n" + ora + ":" + 0 + minuto;
        }
        else
        {
            testoOrario.text= "Giorno " + giorno + "\n" + ora + ":" + minuto;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.time - ultimoScatto)>durataMinuto)
        {
            minuto++;
            if (minuto>60)
            {
                ora++;
                minuto=0;
                if (ora>=24)
                {
                    giorno++;
                    ora=8;
                }
            }
            Stampa();
            ultimoScatto=Time.time;
        }
    }
}
