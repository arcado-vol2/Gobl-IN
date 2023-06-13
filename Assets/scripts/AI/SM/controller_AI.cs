using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class controller_AI : MonoBehaviour
{
    public Transform path_holder;
    [HideInInspector]
    //public Transform player;

    public float speed = 5f;
    public float turn_speed = 130;

    public float patrol_detect_time = 2f;
    public float search_rotate_time = 2f;
    public float search_time = 2f;
    public float defuse_distance = 2f;
    public float defuse_time = 2f;
    public float hear_radius = 100f;
    [HideInInspector]
    public int defuse_stenght = 1;


    public float attack_range = 2f;
    [Range(0, 360)]
    public float attack_angle = 60f;

    [HideInInspector]
    public state_machine_AI SM;
    [HideInInspector]
    public patrol_state_AI s_patrol;
    [HideInInspector]
    public search_state_AI s_search;
    [HideInInspector]
    public chase_state_AI s_chase;
    [HideInInspector]
    public defuse_state_AI s_defuse;

    public field_of_view FOV;
    public NavMeshAgent AI_agent;

    [HideInInspector]
    public Transform target;

    [SerializeField]
    Sprite patrol_image;
    [SerializeField]
    Sprite chase_image;
    [SerializeField]
    Sprite search_image;
    [SerializeField]
    Sprite defuse_image;
    [SerializeField]
    Image state_image;

    [SerializeField]
    level_manager lvl_manager;

    //�� �������, ��� ��� �������� ����� �������, �� ������� � ���� ��� ��� ���
    private void OnDrawGizmos()
    {
        Transform start_point = path_holder.GetChild(0);
        Transform last_point = start_point;
        path_point.point_type last_point_type = path_holder.GetChild(0).GetComponent<path_point>().get_current();
        Color point_color = Color.white;
        foreach (Transform point in path_holder)
        {
            Gizmos.color = Color.white;
            switch (point.GetComponent<path_point>().get_current())
            {
                case path_point.point_type.Regular:
                    point_color = Color.white;
                    Gizmos.DrawLine(last_point.position, point.position);
                    break;
                case path_point.point_type.Wait:
                    point_color = Color.yellow;
                    Gizmos.DrawLine(last_point.position, point.position);
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(point.position, point.position + point.GetComponent<path_point>().get_look_direction());
                    break;
                case path_point.point_type.Reverse:
                    point_color = Color.green;
                    if (last_point_type != path_point.point_type.Reverse)
                    {
                        Gizmos.DrawLine(last_point.position, point.position);
                    }
                    break;
            }
            Gizmos.color = point_color;
            Gizmos.DrawSphere(point.position, 0.3f);

            last_point = point;
            last_point_type = point.GetComponent<path_point>().get_current();
        }
        Gizmos.DrawLine(start_point.position, last_point.position);
    }
    void Start()
    {
        SM_initialize();
        RefreshState();
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
    public IEnumerator FollowPath(Transform[] waypoints)
    {
        Vector3[] waypoints_fixed_pos = new Vector3[path_holder.childCount];
        for (int i = 0; i < waypoints_fixed_pos.Length; i++)
        {
            waypoints_fixed_pos[i] = new Vector3(waypoints[i].position.x, transform.position.y, waypoints[i].position.z);
        }

        int target_waypoint_index = 0;
        float nearest_distance = float.MaxValue;
        for (int i = 0; i < waypoints_fixed_pos.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, waypoints_fixed_pos[i]);

            if (distance < nearest_distance)
            {
                nearest_distance = distance;
                target_waypoint_index = i;
            }
        }
        Vector3 target_waypoint = waypoints_fixed_pos[target_waypoint_index];
        float wait_time = waypoints[target_waypoint_index].GetComponent<path_point>().wait_time;
        sbyte index_modificator = 1;
        AI_agent.SetDestination(target_waypoint);
        while (true)
        {

            if (Mathf.Abs(transform.position.x - target_waypoint.x) <= 0.3 && Mathf.Abs(transform.position.z - target_waypoint.z) <= 0.3)
            {
                wait_time = waypoints[target_waypoint_index].GetComponent<path_point>().wait_time;
                if (waypoints[target_waypoint_index].GetComponent<path_point>().get_current() == path_point.point_type.Reverse)
                {
                    index_modificator *= -1;
                }
                if (waypoints[target_waypoint_index].GetComponent<path_point>().get_current() == path_point.point_type.Wait)
                {
                    yield return StartCoroutine(TurnToFace(transform.position + waypoints[target_waypoint_index].GetComponent<path_point>().get_look_direction()));
                }
                if (index_modificator == -1 && target_waypoint_index <= 0)
                {
                    target_waypoint_index = waypoints.Length;
                }
                target_waypoint_index = Mathf.Abs(index_modificator + target_waypoint_index) % waypoints.Length;
                target_waypoint = waypoints_fixed_pos[target_waypoint_index];
                AI_agent.SetDestination(target_waypoint);
                yield return new WaitForSeconds(wait_time);
                yield return StartCoroutine(TurnToFace(target_waypoint));
            }

            yield return null;
        }
    }

    public IEnumerator GoToPoint(Vector3 target)
    {
        AI_agent.SetDestination(target);
        while (transform.position.x != target.x && transform.position.z != target.z)
        {
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
    IEnumerator TurnToFace(float look_angle)
    {
        float angle = 0;
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, look_angle)) >= 0.08f)
        {
            angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, look_angle, turn_speed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    public IEnumerator Search(Vector3 search_point = new Vector3(), bool crutch = false)
    {
        if (!crutch)
        {
            search_point = target.position;
        }
        yield return StartCoroutine(GoToPoint(search_point));
        while (search_time > 0)
        {
            yield return StartCoroutine(TurnToFace(0));
            yield return new WaitForSeconds(search_rotate_time);
            yield return StartCoroutine(TurnToFace(120));
            yield return new WaitForSeconds(search_rotate_time);
            yield return StartCoroutine(TurnToFace(280));
            yield return new WaitForSeconds(search_rotate_time);
        }
    }
    public IEnumerator Chase_closest_target()
    {
        int target_id = get_closest_target_id();
        while (FOV.visible_targets.Count > 0)
        {
            if (FOV.visible_targets[target_id] == null)
            {
                yield break;
            }
            AI_agent.SetDestination(FOV.visible_targets[target_id].position);
            yield return null;
        }
    }

    public IEnumerator GoToClosetDevise()
    {
        if (FOV.visible_devices_targets.Count <= 0)
        {
            yield break;
        }
        AI_agent.stoppingDistance = defuse_distance;
        while (FOV.visible_devices_targets.Count > 0)
        {
            int target_id = get_closest_target_id(FOV.visible_devices_targets);
            AI_agent.SetDestination(FOV.visible_devices_targets[target_id].position);

            Collider[] devices_in_range = Physics.OverlapSphere(transform.position, defuse_distance, FOV.device_target_mask);
            for (int i = 0; i < devices_in_range.Length; i++)
            {
                device_logic device = devices_in_range[i].GetComponent<device_logic>();
                yield return StartCoroutine(Defuse(device));
                yield return null;
            }
            yield return null;
        }
        AI_agent.stoppingDistance = 0;
    }

    private void SM_initialize()
    {
        SM = new state_machine_AI();
        s_chase = new chase_state_AI(this, SM);
        s_patrol = new patrol_state_AI(this, SM);
        s_search = new search_state_AI(this, SM);
        s_defuse = new defuse_state_AI(this, SM);
        SM.initialize(s_patrol, this);
    }

    public int get_closest_target_id(List<Transform> targets = null)
    {
        if (targets == null)
        {
            targets = FOV.visible_targets;
        }
        int target_id = 0;
        float max_distance = float.MaxValue;
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null)
            {
                continue;
            }
            float distance = Vector3.Distance(transform.position, targets[i].position);
            if (distance < max_distance)
            {
                max_distance = distance;
                target_id = i;
            }
        }
        return target_id;

    }
    public void Attack()
    {
        Collider[] targets_in_attack_range = Physics.OverlapSphere(transform.position, attack_range, FOV.target_mask);
        for (int i = 0; i < targets_in_attack_range.Length; i++)
        {
            Transform target = targets_in_attack_range[i].transform;
            Vector3 direction_to_target = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, direction_to_target) < attack_angle / 2)
            {
                unit_base target_unite_base = targets_in_attack_range[i].GetComponent<unit_base>();
                if (target_unite_base != null)
                {
                    target_unite_base.GetDamage(999);
                }
            }
        }
    }

    public IEnumerator Defuse(device_logic device)
    {
        float wait_time = defuse_time / defuse_stenght;
        while (device != null)
        {
            device.LoseDurability(defuse_stenght);
            yield return new WaitForSeconds(wait_time);
        }
    }


    public void RefreshState()
    {
        if (SM.current_state == s_patrol)
        {
            state_image.enabled = false;
            //state_image.sprite = patrol_image;
        }
        else
        {
            state_image.enabled = true;
            if (SM.current_state == s_search)
            {
                state_image.sprite = search_image;
            }
            else if (SM.current_state == s_chase)
            {
                state_image.sprite = chase_image;
            }
            else if (SM.current_state == s_defuse)
            {
                state_image.sprite = defuse_image;
            }
        }

    }
    public void Die()
    {
        lvl_manager.KillEnemy();
    }


}
