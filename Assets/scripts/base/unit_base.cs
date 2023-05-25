using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit_base : MonoBehaviour
{
    public int health;
    public void GetDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
