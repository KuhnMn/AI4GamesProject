using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{  
    public IdleState idleState;
    public ChaseState chaseState;

    public GameObject agent;

    public bool startIdle;
    public bool startChase;

    public Vector3 startPosition;
    public Vector3 targetPosition;

    private FindEnemies findEnemies;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        this.navMeshAgent = agent.GetComponent<NavMeshAgent>();
        this.findEnemies = agent.GetComponent<FindEnemies>();
    }

    public override State RunCurrentState()
    {
        startChase = false;
        startIdle = false;
        if (findEnemies.enemiesInRange.Count > 0)
        {
            startChase = true;
        }
        if (Input.GetKeyDown("i"))
        {
            startPosition = Vector3.zero;
            startIdle = true;
        }
        if (startIdle)
        {
            startIdle = false;
            return idleState;
        } else if (startChase)
        {
            startChase = false;
            return chaseState;
        }
        else
        {
            if (startPosition == Vector3.zero)
            {
                startPosition = agent.transform.position;
                targetPosition = agent.transform.position + agent.transform.forward * 5f;
            }
            if (Vector3.Distance(agent.transform.position, targetPosition) < 1)
            {
                var tempPosition = targetPosition;
                targetPosition = startPosition;
                startPosition = tempPosition;
            }
            navMeshAgent.SetDestination(targetPosition);
            return this;
        }
    }
}
