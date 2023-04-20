using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class controller_AI : MonoBehaviour
{
    public Transform path_holder;
    [HideInInspector]
    public Transform player;

    public float speed = 5f;
    public float patrol_wait_time = 0.3f;
    public float turn_speed = 130;

    [HideInInspector]
    public state_machine_AI SM;
    [HideInInspector]
    public patrol_state_AI s_patrol;
    [HideInInspector]
    public search_state_AI s_search;
    [HideInInspector]
    public chase_state_AI s_chase;

    //Не сказать, что мне нравится такое решение, но другого у меня для вас нет
    private void OnDrawGizmos()
    {

        Vector3 start_point = path_holder.GetChild(0).position;
        Vector3 last_point = start_point;

        foreach (Transform point in path_holder)
        {
            Gizmos.DrawSphere(point.position, 0.3f);
            Gizmos.DrawLine(last_point, point.position);
            last_point = point.position;
        }
        Gizmos.DrawLine(start_point, last_point);
    }
    void Start()
    {
        SM_initialize();
    }
    void Update()
    {
        SM.current_state.HandleInput();
        SM.current_state.LogicUpdate();
        
    }

    private void FixedUpdate()
    {
        SM.current_state.PhysicsUpdate();
    }
    public IEnumerator FollowPath(Vector3[] waypoints)
    {
        transform.position = waypoints[0];
        int target_waypoint_index = 1;
        Vector3 target_waypoint = waypoints[target_waypoint_index];
        transform.LookAt(target_waypoint);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, target_waypoint, speed * Time.deltaTime);
            if (transform.position == target_waypoint)
            {

                target_waypoint_index = ++target_waypoint_index % waypoints.Length;
                target_waypoint = waypoints[target_waypoint_index];
                yield return new WaitForSeconds(0.3f);
                yield return StartCoroutine(TurnToFace(target_waypoint));
            }
            yield return null;
        }
    }
    IEnumerator TurnToFace(Vector3 target)
    {
        Vector3 direction_to_target = (target - transform.position).normalized;
        float look_angle = 90 - Mathf.Atan2(direction_to_target.z, direction_to_target.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, look_angle)) >= 0.08f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, look_angle, turn_speed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    private void SM_initialize()
    {
        SM = new state_machine_AI();
        s_chase = new chase_state_AI(this, SM);
        s_patrol = new patrol_state_AI(this, SM);
        s_search = new search_state_AI(this, SM);
        SM.initialize(s_patrol);
    }
}
