using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class blast_wave : MonoBehaviour
{
    public int point_count;
    public float max_radius;
    public float min_radius;
    public float speed;
    public float start_width;
    public int blast_damage = 999;
    public GameObject exp_light;
    public ParticleSystem explosion;
    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = point_count + 1;
    }

    private void Start()
    {
        StartCoroutine(Blast());
    }
    private IEnumerator Blast()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= enemy.GetComponent<controller_AI>().hear_radius)
            {
                controller_AI enemy_contoller = enemy.GetComponent<controller_AI>();
                enemy_contoller.target = transform;
                enemy_contoller.SM.change_state(enemy_contoller.s_search);
            }
        }

        float curr_radius = min_radius;
        while (curr_radius< max_radius)
        {
            curr_radius += Time.deltaTime * speed;
            DrawBlast(curr_radius);
            Damage(curr_radius);
            yield return null;
        }
        Destroy(exp_light);
    }
    public void Update()
    {
        if (explosion.particleCount == 0)
        {
            Destroy(gameObject);
        }
    }

    private void DrawBlast(float radius)
    {
        float angle_between_points = 360f / point_count;

        for (int i = 0; i <= point_count; i++)
        {
            float angle = i * angle_between_points * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));
            Vector3 position = direction * radius;
            line.SetPosition(i, position);
        }
        line.widthMultiplier = Mathf.Lerp(0f, start_width, 1f - radius / max_radius);
    }
    private void Damage(float radius)
    {
        Collider[] hitted_targets = Physics.OverlapSphere(transform.position, radius);

        for(int i=0; i<hitted_targets.Length; i++)
        {
            unit_base target = hitted_targets[i].transform.GetComponent<unit_base>();
            if (target != null)
            {
                target.GetDamage(blast_damage);
            }
        }
    }
}
