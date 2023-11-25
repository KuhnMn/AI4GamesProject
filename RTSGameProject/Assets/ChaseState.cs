using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
    public PatrolState patrolState;
    public AttackState attackState;

    public GameObject agent;

    public bool startPatrol;
    public bool startAttack;
    private UnitStats unitStats;
    private FindEnemies findEnemies;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        this.navMeshAgent = agent.GetComponent<NavMeshAgent>();
        this.unitStats = agent.GetComponent<UnitStats>();
        this.findEnemies = agent.GetComponent<FindEnemies>();
    }

    public override State RunCurrentState()
    {
        startAttack = false;
        startPatrol = false;
        Debug.Log(navMeshAgent.stoppingDistance);
        this.navMeshAgent.stoppingDistance = this.unitStats.GetAttackRange();
        if (findEnemies.enemiesInRange.Count == 0)
        {
            startPatrol = true;
        }
        else if (findEnemies.enemiesInRange[0].Item1 <= this.unitStats.GetAttackRange())
        {
            startAttack = true;
        }
        if (startPatrol)
        {
            navMeshAgent.stoppingDistance = 0.0f;
            return patrolState;
        } else if (startAttack)
        {
            return attackState;
        }
        else
        {
            if (findEnemies.enemiesInRange.Count > 0)
            {
                navMeshAgent.SetDestination(findEnemies.enemiesInRange[0].Item2.transform.position);
            }
            return this;
        }
    }
}
