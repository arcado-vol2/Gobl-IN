using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit_base : MonoBehaviour
{
    public int health = 0;

    public void GetDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (gameObject.tag == "Player")
        {
            GetComponent<character_auto_controller>().SaveQueue();
        }
        else if (gameObject.tag == "Enemy")
        {
            Debug.Log("enemy");
        }
        Destroy(gameObject);
    }
}
