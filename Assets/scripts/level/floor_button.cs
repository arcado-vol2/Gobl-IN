using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floor_button : MonoBehaviour
{
    public Door door;

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, door.transform.position + new Vector3(0,1,0));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            door.Open();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            door.Close();
        }
    }
}
