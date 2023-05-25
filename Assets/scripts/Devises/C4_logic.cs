using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C4_logic : MonoBehaviour
{
    public GameObject explosion_prefab;
    public void BlowUp()
    {
        Instantiate(explosion_prefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
