using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class contoller_player : MonoBehaviour
{
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float rotation_speed;

    [Header("Camera")]
    public Camera following_camera;
    private float camera_angle = 45;

    [Header("Character")]
    public bool user_control = true;
    private CharacterController character_controller;
    public float walk_speed = 5f;
    public float walk_rotation_speed = 720f;
    public float sneak_speed = 2f;
    public float sneak_rotation_speed = 500f;
    public float run_speed = 8f;
    public float run_rotation_speed = 1000f;


    [Header("Prefabs")]
    public GameObject mine;
    public GameObject C4;
    public InvertiryManager invertiry_manager;

    [HideInInspector]
    public state_machine_player SM;
    [HideInInspector]
    public move_state_player s_move;
    [HideInInspector]
    public run_state_player s_run;
    [HideInInspector]
    public sneak_state_player s_sneak;
    [HideInInspector]
    public shoot_state_player s_shoot;

    [Header("Aim")]
    private bool aim = false;
    [SerializeField] private LayerMask ground_mask;

    [Header("Laser")]
    [SerializeField] private LineRenderer laser_renderer;
    [SerializeField] private LayerMask laser_mask;
    [SerializeField] private float laser_length;

    private Camera main_camera;

    void Start()
    {
        ToggleAim();
        main_camera = Camera.main;
        SM_initialize();
        camera_angle = 360 - following_camera.transform.localEulerAngles.y;
        character_controller = GetComponent<CharacterController>();
        if (!user_control)
        {
            StartCoroutine(DemoAction());
        }

    }
    void Update()
    {
        if (user_control)
        {
            SM.CurrentState.HandleInput();
            SM.CurrentState.LogicUpdate();
            SM.CurrentState.PhysicsUpdate();
        }
        
    }

    public void Move(float hor_input, float vert_input)
    {
        Vector3 velocity = Quaternion.AngleAxis(-camera_angle, Vector3.up) * new Vector3(hor_input, 0, vert_input);
        velocity.Normalize();
        character_controller.SimpleMove(velocity * Mathf.Clamp01(velocity.magnitude) * speed);

        if (velocity != Vector3.zero)
        {
            Quaternion target_rotation = Quaternion.LookRotation(velocity, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target_rotation, rotation_speed * Time.deltaTime);
        }
    }

    
    private void SM_initialize()
    {
        SM = new state_machine_player();
        s_move = new move_state_player(this, SM);
        s_sneak = new sneak_state_player(this, SM);
        s_run = new run_state_player(this, SM);
        s_shoot = new shoot_state_player(this, SM);

        SM.initialize(s_move);
    }
    public void Aim(bool handle = true, float angle = 0)
    {
        if (handle)
        {
            var (success, position) = GetMousePosition();
            if (success)
            {
                var direction = position - transform.position;
                direction.y = 0;
                transform.forward = direction;
            }
        }
        else
        {
            transform.forward = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));   

        }
        
    }

    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = main_camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, ground_mask))
        {
            return (success: true, position: hitInfo.point);
        }
        else
        {
            return (success: false, position: Vector3.zero);
        }
    }

    public void RefreshLaser()
    {
        if (laser_renderer == null)
        {
            return;
        }

        Vector3 line_end;

        if (Physics.Raycast(transform.position, transform.forward, out var hitinfo, laser_length, laser_mask))
        {
            line_end = hitinfo.point;
        }
        else
        {
            line_end = transform.position + transform.forward * laser_length;
        }
        line_end = new Vector3(line_end.x, laser_renderer.GetPosition(0).y, line_end.z);
        laser_renderer.SetPosition(0, new Vector3(transform.position.x, laser_renderer.GetPosition(0).y, transform.position.z));
        laser_renderer.SetPosition(1, line_end);
    }

    public void ToggleAim()
    {
        laser_renderer.enabled = aim;
        aim = !aim;
    }

    public void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, laser_length))
        {
            unit_base target = hit.transform.GetComponent<unit_base>();
            if (target != null)
            {
                target.GetDamage(999);
            }
        }

    }

    public void Detonate()
    {
        GameObject[] bombs = GameObject.FindGameObjectsWithTag("C4");
        foreach (GameObject go in bombs)
        {
            go.GetComponent<C4_logic>().BlowUp();
        }
    }

    public void SpawDevice(GameObject device_prefab)
    {
        Instantiate(device_prefab, transform.position, transform.rotation);
    }

    public action[] action_list;
    
    

    public IEnumerator DemoAction()
    {
        float current_time = 0;
        int current_action_index = 0;
        bool has_done = false;
        while (current_action_index < action_list.Length)
        {
            current_time += Time.deltaTime;
            if (action_list[current_action_index].long_action)
            {
                switch (action_list[current_action_index].type)
                {
                    case DemoActionType.move:
                        Move(action_list[current_action_index].hor_input, action_list[current_action_index].vert_input);
                        Debug.Log("auto");
                        break;
                    case DemoActionType.plant:
                        switch (action_list[current_action_index].device)
                        {
                            case DemoDeviceType.mine:
                                SpawDevice(mine);
                                break;
                            case DemoDeviceType.C4:
                                SpawDevice(C4);
                                break;
                        }
                        break;
                }
            }
            else if (!has_done)
            {
                has_done = true;
                switch (action_list[current_action_index].type)
                {
                    case DemoActionType.plant:
                        switch (action_list[current_action_index].device)
                        {
                            case DemoDeviceType.mine:
                                SpawDevice(mine);
                                break;
                            case DemoDeviceType.C4:
                                SpawDevice(C4);
                                break;
                        }
                        break;
                    case DemoActionType.blow_up:
                        Detonate();
                        break;
                    case DemoActionType.attack:
                        Aim(false, action_list[current_action_index].angle);
                        break;
                }
            }
            if (current_time >= action_list[current_action_index].time)
            {
                
                current_action_index++;
                has_done = false;
            }
            yield return null;
        }
        
    }

}
