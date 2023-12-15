using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    public FindEnemies findEnemies;
    private NavMeshAgent navMeshAgent;
    public GameObject agent;
    public IdleState idleState;
    public ChaseState chaseState;
    public FormationState formationState;
    public float patrolRadius = 10f;
    public float patrolTimeout = 10.0f;
    public bool returnToFormation;

    public bool startIdle;
    public bool startChase;

    private Vector3 startPosition;
    private Vector3 currentTargetPosition;
    private float timer;

    void Start()
    {
        this.navMeshAgent = agent.GetComponent<NavMeshAgent>();
        this.returnToFormation = false;
    }

    public override State RunCurrentState()
    {
        timer += Time.deltaTime;
        if (returnToFormation)
        {
            returnToFormation = false;
            return formationState;
        }
        if (findEnemies.enemiesInRange.Count > 0)
        {
            return chaseState;
        }
        else
        {
            if (startPosition == Vector3.zero)
            {
                startPosition = agent.transform.position;
                currentTargetPosition = GetRandomPatrolPosition();
            }
            if (Vector3.Distance(agent.transform.position, currentTargetPosition) < 1)
            {
                currentTargetPosition = GetRandomPatrolPosition();
            }
            else if (timer > patrolTimeout)
            {
                timer = 0;
                currentTargetPosition = GetRandomPatrolPosition();
            }
            navMeshAgent.SetDestination(currentTargetPosition);
            return this;
        }
    }

    private Vector3 GetRandomPatrolPosition()
    {
        float randomAngle = Random.Range(0, 360); // Generate a random angle in degrees
        float randomRadius = Random.Range(0, patrolRadius); // Generate a random distance within the scouting radius

        // Convert polar coordinates to Cartesian coordinates
        float x = startPosition.x + randomRadius * Mathf.Cos(Mathf.Deg2Rad * randomAngle);
        float z = startPosition.z + randomRadius * Mathf.Sin(Mathf.Deg2Rad * randomAngle);

        return new Vector3(x, startPosition.y, z);
    }
}
