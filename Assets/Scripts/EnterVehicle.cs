using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterVehicle : MonoBehaviour {

    public GameObject prompt;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name.Equals("Soldier"))
            (prompt.GetComponent("SpriteRenderer") as SpriteRenderer).enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Soldier"))
            (prompt.GetComponent("SpriteRenderer") as SpriteRenderer).enabled = false;
    }

}
