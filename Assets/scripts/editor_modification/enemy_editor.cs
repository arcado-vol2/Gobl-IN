using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(controller_AI))]
public class enemy_editor : Editor
{
    private void OnSceneGUI()
    {
        controller_AI fow = (controller_AI)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.hear_radius);
    }
}