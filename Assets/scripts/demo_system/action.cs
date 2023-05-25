using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable object/action")]
public class action : ScriptableObject
{
    public DemoActionType type;
    public float vert_input;
    public float hor_input;
    public DemoDeviceType device; 
    public float time;
    [Range(0, 360)]
    public float angle;
    public bool long_action;
}

public enum DemoActionType
{
    move,
    plant,
    attack,
    blow_up
}   

public enum DemoDeviceType
{
    mine,
    C4
}