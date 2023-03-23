using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class player_controller : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;
    [SerializeField]
    private float rotation_speed = 720.0f;
    public Camera following_camera;
    private float camera_angle = 45;
    private CharacterController character_controller;
    
    void Start()
    {
        camera_angle = 360 - following_camera.transform.localEulerAngles.y;
        character_controller = GetComponent<CharacterController>();
        Debug.Log(camera_angle);
    }
    void Update()
    {
        float horizontal_input = Input.GetAxis("Horizontal");
        float vertical_input = Input.GetAxis("Vertical");
        
        Vector3 velocity = Quaternion.AngleAxis(-camera_angle, Vector3.up) * new Vector3(horizontal_input, 0, vertical_input);
        velocity.Normalize();
        character_controller.SimpleMove(velocity * Mathf.Clamp01(velocity.magnitude) *  speed);

        if (velocity != Vector3.zero){
            Quaternion target_rotation = Quaternion.LookRotation(velocity, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target_rotation, rotation_speed * Time.deltaTime);
        }
    }
}
