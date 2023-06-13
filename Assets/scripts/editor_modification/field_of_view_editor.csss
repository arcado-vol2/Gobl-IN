using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (field_of_view))]
public class field_of_view_editor : Editor
{
    private void OnSceneGUI()
    {
        field_of_view fow = (field_of_view)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.view_radius);
        Vector3 view_angle_1 = fow.AngleToDirection(-fow.view_angle / 2, false);
        Vector3 view_angle_2 = fow.AngleToDirection(fow.view_angle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + view_angle_1 * fow.view_radius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + view_angle_2 * fow.view_radius);

        Handles.color = Color.cyan;
    }
}
