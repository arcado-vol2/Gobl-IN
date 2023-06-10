using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System;

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
    public GameObject body_obj;

    [Header("Prefabs")]
    public GameObject mine;
    public GameObject C4;
    public InvertiryManager invertiry_manager;

    //State machine
    [HideInInspector]
    public state_machine_player SM;
    [HideInInspector]
    public character_auto_controller CAC;
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
    private wall_key_button terminal;


    void Start()
    {
        ToggleAim();
        main_camera = Camera.main;
        CAC = GetComponent<character_auto_controller>();
        SM_initialize();
        camera_angle = 360 - following_camera.transform.localEulerAngles.y;
        character_controller = GetComponent<CharacterController>();

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
        s_move = new move_state_player(this, SM, CAC);
        s_sneak = new sneak_state_player(this, SM, CAC);
        s_run = new run_state_player(this, SM, CAC);
        s_shoot = new shoot_state_player(this, SM, CAC);

        SM.initialize(s_move);
    }
    public void Aim(bool handle = true, float angle = 0)
    {
        if (handle)
        {
            var (success, position) = GetMousePosition();
            if (success)
            {
                var direction = position - body_obj.transform.position;
                direction.y = 0;
                body_obj.transform.right = -direction;
                //ody_obj.transform.Rotate(new Vector3(0, 1, 0), 90);
            }
        }
        else
        {
            body_obj.transform.forward = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));    

        }
        
    }
    public void ResetAim()
    {
        body_obj.transform.forward = transform.right;
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

        if (Physics.Raycast(body_obj.transform.position, -body_obj.transform.right, out var hitinfo, laser_length, laser_mask))
        {
            line_end = hitinfo.point;
        }
        else
        {
            line_end = body_obj.transform.position + (-body_obj.transform.right) * laser_length;
        }
        line_end = new Vector3(line_end.x, laser_renderer.GetPosition(0).y, line_end.z);
        laser_renderer.SetPosition(0, new Vector3(body_obj.transform.position.x, laser_renderer.GetPosition(0).y, body_obj.transform.position.z));
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
        if (Physics.Raycast(body_obj.transform.position, -body_obj.transform.right, out hit, laser_length))
        {
            unit_base target = hit.transform.GetComponent<unit_base>();
            if (target != null)
            {
                target.GetDamage(999);
            }
        }
        ResetAim();
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

    private void OnTriggerEnter(Collider other)
    {
        wall_key_button tmp = other.GetComponent<wall_key_button>();
        if (tmp != null)
        {
            terminal = tmp;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<wall_key_button>() != null)
        {
            terminal = null;
        }
    }


    public bool use_key()
    {

        if (terminal != null)
        {
            terminal.use_key();
            return true;
        }
        return false;
    }

}
