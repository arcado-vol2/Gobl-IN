using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static field_of_view;

public class field_of_view : MonoBehaviour
{
    public float view_radius; 
    [Range(0,360)]
    public float view_angle;

    public LayerMask target_mask;
    public LayerMask device_target_mask;
    public LayerMask wall_mask;

    [HideInInspector]
    public List<Transform> visible_targets = new List<Transform>();

    [HideInInspector]
    public List<Transform> visible_devices_targets = new List<Transform>();

    public float bias;
    public int edge_resolve_iterations;
    //мб надо поменять, решение слишеком в лоб и может вызвать проблемы потом
    public float edge_distance_treshhold;

    public MeshFilter view_mesh_filter;
    Mesh view_mesh;

    private void Start()
    {
        view_mesh = new Mesh();
        view_mesh.name = "View mesh";
        view_mesh_filter.mesh = view_mesh;

        StartCoroutine(FindTargetsWithDelay(.2f));
        StartCoroutine(FindDevicesWithDelay(.2f));
    }
    private void Update()
    {
        DrawFOW();
    }
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisilbleTargets(target_mask, ref visible_targets);
        }
    }

    IEnumerator FindDevicesWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisilbleTargets(device_target_mask, ref visible_devices_targets);
        }
    }

    void FindVisilbleTargets(LayerMask search_mask, ref List<Transform> out_list)
    {
        out_list.Clear();
        Collider[] targets_in_view_radius = Physics.OverlapSphere(transform.position, view_radius, search_mask);

        for ( int i=0; i< targets_in_view_radius.Length; i++)
        {
            Transform target = targets_in_view_radius[i].transform;
            Vector3 direction_to_target = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, direction_to_target) < view_angle/2)
            {
                
                float disance_to_target = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, direction_to_target, disance_to_target, wall_mask))
                {
                    out_list.Add(target);
                }

            }
        }
    }


    void DrawFOW()
    {
        int steps_count = Mathf.RoundToInt( view_angle * bias);
        float step_size = view_angle / steps_count;
        List<Vector3> view_points = new List<Vector3>();
        RayCastFOWInfo old_raycast = new RayCastFOWInfo();
        for (int i = 0; i <= steps_count; i++)
        {
            float angle = transform.eulerAngles.y - view_angle / 2 + step_size * i;
            RayCastFOWInfo new_raycast = RayCastFOW(angle);
            if (i > 0)
            {
                bool edge_dist_TH_exceeded = Mathf.Abs(old_raycast.size - new_raycast.size) > edge_distance_treshhold;
                if (old_raycast.hit != new_raycast.hit || (old_raycast.hit && new_raycast.hit && edge_dist_TH_exceeded))
                {
                    EdgeInfo edge = FindEdge(old_raycast, new_raycast);
                    if (edge.point_1 != Vector3.zero)
                    {
                        view_points.Add(edge.point_1);
                    }
                    if (edge.point_2 != Vector3.zero)
                    {
                        view_points.Add(edge.point_2);
                    }
                }
            }
            view_points.Add(new_raycast.point);
            old_raycast = new_raycast;
        }

        int vertex_count = view_points.Count + 1;
        Vector3[] vertices = new Vector3[vertex_count];
        int[] triangles = new int[(vertex_count - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertex_count - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint( view_points[i]);

            if (i < vertex_count - 2) 
            { 
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        view_mesh.Clear();
        view_mesh.vertices = vertices;
        view_mesh.triangles = triangles;
        view_mesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(RayCastFOWInfo min_raycast, RayCastFOWInfo max_raycast)
    {
        float min_angle = min_raycast.angle;
        float max_angle = max_raycast.angle;
        Vector3 min_point = Vector3.zero;
        Vector3 max_point = Vector3.zero;

        for (int i = 0; i< edge_resolve_iterations; i++)
        {
            float angle = (min_angle + max_angle) / 2;
            RayCastFOWInfo new_raycast = RayCastFOW(angle);

            bool edge_dist_TH_exceeded = Mathf.Abs(min_raycast.size - new_raycast.size) > edge_distance_treshhold;
            if (new_raycast.hit == min_raycast.hit && !edge_dist_TH_exceeded)
            {
                min_angle = angle;
                min_point = new_raycast.point;
            }
            else
            {
                max_angle = angle;
                max_point = new_raycast.point;
            }
        }
        return new EdgeInfo(min_point, max_point);

    }


    RayCastFOWInfo RayCastFOW(float global_angle)
    {
        Vector3 direction = AngleToDirection(global_angle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, view_radius, wall_mask))
        {
            return new RayCastFOWInfo(true, hit.point, hit.distance, global_angle);
        }
        else
        {
            return new RayCastFOWInfo(false, transform.position + direction * view_radius, view_radius, global_angle);
        }
    }
    /// <summary>
    /// Метод преобразующий угол в вектор направления.
    /// 
    /// Угол указывается в градусах
    /// </summary>
    public Vector3 AngleToDirection(float angle, bool angle_is_global)
    {
        if (!angle_is_global )
        {
            angle += transform.eulerAngles.y;
        }
        //Замечу, что в Uniy Pi/2 - 0 градусов, поэтому sin и cos меняются местами
        angle *= Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
        
    }

    public struct RayCastFOWInfo
    {
        public bool hit;
        public Vector3 point;
        public float size;
        public float angle;

        public RayCastFOWInfo(bool __hit, Vector3 __point, float __size, float __angle)
        {
            hit = __hit;
            point = __point;
            size = __size;
            angle = __angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 point_1;
        public Vector3 point_2;

        public EdgeInfo(Vector3 __point_1, Vector3 __point_2)
        {
            point_1 = __point_1;
            point_2 = __point_2;

        }
    }
}
