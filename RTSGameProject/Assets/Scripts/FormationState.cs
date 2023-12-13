using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FormationState : State
{
    public State chaseState;
    public ScoutState scoutState;
    public GameObject agent;
    public Vector3 formationGoal;
    public float speed;
    public Boolean forceFormation = false;
    public Vector3 foundEnemy;

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
        if (forceFormation)
        {
            return this;
        }
        else if (findEnemies.enemiesInRange.Count > 0)
        {
            this.navMeshAgent.speed = unitStats.GetMoveSpeed();
            this.unitStats.InFormation.GetComponent<Formation>().breakFormation(findEnemies.enemiesInRange[0].Item2.transform.position);
            return chaseState;
        }
        else if (foundEnemy != Vector3.zero)
        {
            this.scoutState.scoutLocation = foundEnemy;
            foundEnemy = Vector3.zero;
            return scoutState;
        }
        else
        {
            return this;
        }

    }
}
