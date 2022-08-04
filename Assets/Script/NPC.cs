using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Collidable
{
    /* Classe creata per parlare con gli NPC 
        Si pensa di cambiarne il nome in "Talker", creare un GameObject figlio a cui legare Talker 
        e creare la classe NPC come figlia di Mover
    */

    public string nome = "NPC a caso";                                                      // Nome dell'NPC
    public string[] frasi = new string[] {"Ah ma sei tu!", "Adesso mi tocca incularti"};    // Frasi che dirà (Si pensa di cambiare in una lista di vettori di stringhe per dare varietà)

    
    protected override void OnCollide(Collider2D coll){

        if (Input.GetKeyDown(KeyCode.Space)){
            if (coll.name == "Player"){
                GameManager.instanza.MostraConversationText(frasi,nome);

            } 
        }
    }
}
