using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patrol
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attack 
    public float AttackTime;
    bool checkAttack;

    //States
    public float sightRange, attackRange;
    public bool checkSightRange, checkAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Slime").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        checkSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        checkAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!checkSightRange && !checkAttackRange) Patrol();
        if (checkSightRange && !checkAttackRange) Chase();
        if (checkSightRange && checkAttackRange) Attack();
    }

    private void Patrol()
    {
        if (!walkPointSet) SearchWalkPoint();

        if(walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
    private void Attack()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if(!checkAttack)
        {
            //write an attack 

            //end an attack
            checkAttack = true;
            Invoke(nameof(checkAttack), AttackTime);
        }
    }
    private void ResetAttach()
    {
        checkAttack = false;
    }
    private void Chase()
    {
        agent.SetDestination(player.position);
    }
}
