using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FormationState : State
{
    public State chaseState;
    public State patrolState;
    public GameObject agent;
    public Vector3 formationGoal;
    public float speed;
    public Boolean forceFormation = false;

    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private FindEnemies findEnemies;
    private UnitStats unitStats;

    void Start()
    {
        this.findEnemies = agent.GetComponent<FindEnemies>();
        this.navMeshAgent = agent.GetComponent<NavMeshAgent>();
        this.unitStats = agent.GetComponent<UnitStats>();
    }

    public override State RunCurrentState()
    {
        this.navMeshAgent.speed = this.speed;
        this.navMeshAgent.SetDestination(formationGoal);
        if (findEnemies.enemiesInRange.Count == 0 || forceFormation)
        {
            return this;
        }
        else
        {
            this.navMeshAgent.speed = unitStats.GetMoveSpeed();
            return chaseState;
        }
    }
}
