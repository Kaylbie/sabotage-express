using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackRate = 2f;
    public int attackDamage = 10;
    public float wanderRadius = 7f; 
    public float wanderInterval = 4f; 

    private Transform targetPlayer;
    private NavMeshAgent agent;
    private float lastAttackTime;
    private float lastWanderTime = 0f; 

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        targetPlayer = FindClosestPlayer();

        if (targetPlayer != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);
            if (distanceToPlayer <= detectionRange)
            {
                MoveTowardsTarget(targetPlayer);
                if (distanceToPlayer <= attackRange)
                {
                    AttemptAttack(targetPlayer);
                }
            }
            else
            {
                Wander();
            }
        }
    }

    Transform FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(player.transform.position, currentPosition);
            if (distance < closestDistance)
            {
                closestPlayer = player.transform;
                closestDistance = distance;
            }
        }

        return closestPlayer;
    }

    void MoveTowardsTarget(Transform target)
    {
        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    void AttemptAttack(Transform target)
    {
        if (Time.time - lastAttackTime >= attackRate)
        {
            lastAttackTime = Time.time;
            Debug.Log($"Enemy attacks {target.name}!");
            // attack logic 
        }
    }

    void Wander()
    {
        if (Time.time - lastWanderTime >= wanderInterval)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            lastWanderTime = Time.time;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
