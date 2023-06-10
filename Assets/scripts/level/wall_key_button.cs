using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall_key_button : MonoBehaviour
{
    public Door door;

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, door.transform.position + new Vector3(0, 1, 0));
    }
    public void use_key()
    {
        door.Open();
    }
}
