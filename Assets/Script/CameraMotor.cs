using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{

    public Transform lookAt;        // La camera segue il giocatore, memorizziamo il transform del giocatore per poterne prendere la posizione
    private Vector3 moveDelta;

    private void Start(){
        lookAt=GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        moveDelta = new Vector3(0,0,0);
        moveDelta = lookAt.position-transform.position; //vettore congiungente camera e giocatore
        moveDelta.z = 0;                                //impostiamo z=0 perchè non vogliamo che la camera si avvicini al piano di gioco ma che rimanga a distanza costante (facciamo cioè una proiezione del vettore precedente)
        transform.Translate(moveDelta);
    }
}
