using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_conrol : MonoBehaviour
{
    [SerializeField]
    private Transform follow_target;
    [SerializeField]
    private float follow_speed = 2;
    [SerializeField]
    private float x_follow_distance = 5f;
    [SerializeField]
    private float z_follow_distance = 5f;
    [SerializeField]
    private float treshhold = 0.02f;

    void Update()
    {
        float x_current_distance = transform.position.x - follow_target.position.x;
        float z_current_distance = transform.position.z - follow_target.position.z;

        Vector3 new_position = transform.position;

        float x_treshhold = Mathf.Abs(x_current_distance - x_follow_distance);
        float z_treshhold = Mathf.Abs(z_current_distance - z_follow_distance);

        if (x_treshhold > treshhold)
        {
            if (x_current_distance > x_follow_distance)
            {
                new_position.x -= transform.right.x;
            }
            else if (x_current_distance < x_follow_distance)
            {
                new_position.x += transform.right.x;
            }
        }

        if(z_treshhold > treshhold)
        {
            if (z_current_distance > z_follow_distance)
            {
                new_position.z -= transform.forward.z;
            }
            else
            {
                new_position.z += transform.forward.z;
            }
        }

       transform.position = Vector3.Lerp(transform.position, new_position, follow_speed * Time.deltaTime);
    }

  
}
