using UnityEngine;
[System.Serializable]
public class action
{
    public DemoActionType type;
    public float vert_input = 0;
    public float hor_input = 0;
    public DemoDeviceType device = DemoDeviceType.none;
    public float time;
    public Vector3 angle;
    public bool long_action;

    public action(DemoActionType _type, float _v_in, float _h_in, float _time, DemoDeviceType _device, Vector3 _angle, bool _long_action)
    {
        this.type = _type;
        this.vert_input = _v_in;
        this.hor_input = _h_in;
        this.time = _time;
        this.angle = _angle;
        this.long_action = _long_action;
        this.device = _device;
        this.long_action = _long_action;


    }
}


public enum DemoActionType
{
    move,
    plant,
    aim,
    shoot,
    blow_up,
    use,
    none,
}

public enum DemoDeviceType
{
    mine,
    C4,
    none
}