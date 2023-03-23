using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guard : MonoBehaviour
{
    public Transform path_holder;
    Transform player;

    public float speed = 5f;
    public float wait_time = 0.3f;
    public float turn_speed = 130;

    //�� �������, ��� ��� �������� ����� �������, �� ������� � ���� ��� ��� ���
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
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3[] waypoints = new Vector3[path_holder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = new Vector3(path_holder.GetChild(i).position.x, transform.position.y, path_holder.GetChild(i).position.z);
        }
        StartCoroutine(FollowPath(waypoints));
    }
    IEnumerator FollowPath(Vector3[] waypoints)
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
}
