using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class contoller_player : MonoBehaviour
{
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float rotation_speed;
    public Camera following_camera;
    private float camera_angle = 45;
    private CharacterController character_controller;

    public float walk_speed = 5f;
    public float walk_rotation_speed = 720f;
    public float sneak_speed = 2f;
    public float sneak_rotation_speed = 500f;
    public float run_speed = 8f;
    public float run_rotation_speed = 1000f;

    [HideInInspector]
    public state_machine_player SM;
    [HideInInspector]
    public move_state_player s_move;
    [HideInInspector]
    public run_state_player s_run;
    [HideInInspector]
    public sneak_state_player s_sneak;

    void Start()
    {
        SM_initialize();
        camera_angle = 360 - following_camera.transform.localEulerAngles.y;
        character_controller = GetComponent<CharacterController>();
        Debug.Log(camera_angle);
    }
    void Update()
    {
        SM.CurrentState.HandleInput();
        SM.CurrentState.LogicUpdate();
        SM.CurrentState.PhysicsUpdate();
    }

    private void FixedUpdate()
    {
        
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

        SM.initialize(s_move);
    }
}
