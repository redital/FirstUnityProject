using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talker : Collidable
{

    /* Classe creata per parlare con qualsiasi entità parlante 
        Questo script va legato ad un GameObject figlio dell'oggetto parlante
    */

    public string nome = "NPC a caso";                                                      // Nome dell'oggetto parlante
    public string[] frasi = new string[] {"Ah ma sei tu!", "Adesso mi tocca incularti"};    // Frasi che dirà (Si pensa di cambiare in una lista di vettori di stringhe per dare varietà)

    // Start is called before the first frame update
    protected override void Start()
    {
        boxCollider=transform.GetComponent<BoxCollider2D>(); 
        nome = transform.name;
    }

    
    protected override void OnCollide(Collider2D coll){
        if (Input.GetKeyDown(KeyCode.Space) & !GameManager.instanza.combatStatus){
            if (coll.name == GameManager.instanza.player.name){
                GameManager.instanza.MostraConversationText(frasi,nome);

            } 
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
