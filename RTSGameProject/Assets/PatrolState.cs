using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    public FindEnemies findEnemies;
    public NavMeshAgent navMeshAgent;
    public GameObject agent;
    public IdleState idleState;
    public ChaseState chaseState;

    public bool startIdle;
    public bool startChase;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    void start()
    {
        this.agent = GameObject.Find("BattleBean");
        this.navMeshAgent = agent.GetComponent<NavMeshAgent>();
    }

    public override State RunCurrentState()
    {
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
