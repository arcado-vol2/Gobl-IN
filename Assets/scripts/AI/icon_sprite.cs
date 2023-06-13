using UnityEngine;

public class icon_sprite : MonoBehaviour
{
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(Vector3.up, 180f);
    }
}
