using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_base : MonoBehaviour, unit_base
{
    public int health = 0;
    public void GetDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            StartCoroutine(Shrink());
        }
    }

    public void Die()
    {
        GetComponent<controller_AI>().Die();

        Destroy(gameObject);
    }

    IEnumerator Shrink()
    {
        Vector3 start_scale = transform.localScale;
        for (float i = 0; i < 1; i += Time.deltaTime / 0.25f)
        {
            transform.localScale = Vector3.Lerp(start_scale, Vector3.zero, i);
            yield return null;
        }
        transform.localScale = Vector3.zero;
        Die();
    }
}