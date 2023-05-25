using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mine_logic : MonoBehaviour
{
    public GameObject explosion_prefab;
    public LayerMask enemy_layer;
    public float activation_radius;
    public void BlowUp()
    {
        Instantiate(explosion_prefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, activation_radius, enemy_layer);
        if (colliders.Length > 0 )
        {
            BlowUp();
        }
    }
}
