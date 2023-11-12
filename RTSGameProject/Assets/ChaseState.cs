using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
    public PatrolState patrolState;
    public AttackState attackState;
    public FindEnemies findEnemies;
    public NavMeshAgent navMeshAgent;
    public GameObject agent;

    public bool startPatrol;
    public bool startAttack;

    void start()
    {
        this.agent = GameObject.Find("BattleBean");
        this.navMeshAgent = agent.GetComponent<NavMeshAgent> ();
    }

    public override State RunCurrentState()
    {
        if (findEnemies.enemiesInRange.Count == 0)
        {
            startPatrol = true;
        }
        if (Input.GetKeyDown("a"))
        {
            startAttack = true;
        }
        if (startPatrol)
        {
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
