using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    NavMeshObstacle AI_obstacle;
    private void Awake()
    {
        animator= GetComponent<Animator>();
    }
    public void Open()
    {
        AI_obstacle.enabled= false;
        animator.SetBool("Open", true);
    }
    public void Close()
    {
        AI_obstacle.enabled = true;
        animator.SetBool("Open", false);
    }
}
