using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class path_point : MonoBehaviour
{
    public enum point_type {Regular, Wait, Reverse};
    public point_type current = point_type.Regular;

    public float wait_time = 0f;
    [Range(0, 360)]
    public float look_angle = 0f;
    public point_type get_current() { return current; }

    public Vector3 get_look_direction()
    {
        return new Vector3(Mathf.Cos(look_angle * Mathf.Deg2Rad), 0, Mathf.Sin(look_angle * Mathf.Deg2Rad));
    }
}
