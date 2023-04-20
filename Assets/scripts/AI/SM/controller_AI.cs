using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class controller_AI : MonoBehaviour
{
    public Transform path_holder;
    [HideInInspector]
    public Transform player;

    public float speed = 5f;
    public float patrol_wait_time = 0.3f;
    public float turn_speed = 130;

    public float patrol_detect_time = 2f;
    public float chase_interest_time = 2f;
    public float search_time = 2f;

    [HideInInspector]
    public state_machine_AI SM;
    [HideInInspector]
    public patrol_state_AI s_patrol;
    [HideInInspector]
    public search_state_AI s_search;
    [HideInInspector]
    public chase_state_AI s_chase;

    public field_of_view FOV;
    public NavMeshAgent AI_agent;
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
        
        int target_waypoint_index = 0;
        float nearest_distance = float.MaxValue;
        for (int i = 0; i < waypoints.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, waypoints[i]);

            if (distance < nearest_distance)
            {
                nearest_distance = distance;
                target_waypoint_index = i;
            }
        }

        Vector3 target_waypoint = waypoints[target_waypoint_index];

        while (true)
        {
            AI_agent.SetDestination(target_waypoint);
            if (transform.position.x == target_waypoint.x && transform.position.z == target_waypoint.z)
            {
                target_waypoint_index = ++target_waypoint_index % waypoints.Length;
                target_waypoint = waypoints[target_waypoint_index];
                yield return new WaitForSeconds(patrol_wait_time);
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
    public IEnumerator Chase_closest_target()
    {
        int target_id = get_closest_target_id();
        while (FOV.visible_targets.Count > 0)
        {
            AI_agent.SetDestination(FOV.visible_targets[target_id].position);
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

    public int get_closest_target_id()
    {
        int target_id = 0;
        float max_distance = float.MaxValue;
        for (int i =0; i< FOV.visible_targets.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, FOV.visible_targets[i].position);
            if (distance < max_distance) {
                max_distance = distance;
                target_id = i;
            }
        }
        return target_id;

    }


}
