using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class dropped_item : MonoBehaviour
{
    public Item key_scriptable_ob;


    private void OnTriggerEnter(Collider other)
    {
        Player_invertory character = other.GetComponent<Player_invertory>();
        if (character != null)
        {
            character.Add_item(key_scriptable_ob);
            Destroy(gameObject);
        }
    }
}
