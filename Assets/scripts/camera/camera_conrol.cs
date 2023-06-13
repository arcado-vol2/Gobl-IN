using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_conrol : MonoBehaviour
{
    public Transform follow_target;
    private Vector3 offset;
    private Vector3 current_velocity = Vector3.zero;
    [SerializeField]
    private float smooth_time = 0;

    void Awake()
    {
        offset = transform.position - follow_target.position;
        offset.x = 0;
    }

    void LateUpdate()
    {
        if (follow_target != null)
        {
            Vector3 target_position = follow_target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, target_position, ref current_velocity, smooth_time);
        }
    }


}
