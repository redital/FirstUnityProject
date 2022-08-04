using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    protected BoxCollider2D BoxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;

    

    protected virtual void Start(){
        BoxCollider = GetComponent<BoxCollider2D>();
        
    }


    protected virtual void UpdateMotor(Vector3 input){
        //Reset moveDelta
        moveDelta=input;

        //flip del personaggio a seconda che si vada a destra o a sinistra
        if(moveDelta.x>0){
            transform.localScale = new Vector3(1,1,1);
        }
        else{
            if (moveDelta.x<0){ 
                transform.localScale = new Vector3(-1,1,1);
            }
        }

        //controllo collisioni asse y
        hit=Physics2D.BoxCast(transform.position,BoxCollider.size,0,new Vector2(0,moveDelta.y),Mathf.Abs(moveDelta.y*Time.deltaTime),LayerMask.GetMask("Actor","Blocking"));

        if (hit.collider==null){
            transform.Translate(0,moveDelta.y*Time.deltaTime,0);        
        }

        //controllo collisioni asse x
        hit=Physics2D.BoxCast(transform.position,BoxCollider.size,0,new Vector2(moveDelta.x,0),Mathf.Abs(moveDelta.x*Time.deltaTime),LayerMask.GetMask("Actor","Blocking"));

        if (hit.collider==null){
            transform.Translate(moveDelta.x*Time.deltaTime,0,0);        
        }
    }
}