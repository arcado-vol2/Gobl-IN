using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(mine_logic))]
public class mine_editor : Editor
{
    private void OnSceneGUI()
    {
        mine_logic fow = (mine_logic)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.activation_radius);
    }
}