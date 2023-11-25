using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : State
{
    public ChaseState chaseState;
    
    public GameObject agent;

    private float attackTimer;
    private GameObject exclamationMark;
    private UnitStats unitStats;
    private FindEnemies findEnemies;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    void Start()
    {
        this.exclamationMark = agent.transform.Find("Exclamation Mark").gameObject;
        this.unitStats = agent.GetComponent<UnitStats>();
        this.findEnemies = agent.GetComponent<FindEnemies>();
        this.navMeshAgent = agent.GetComponent<NavMeshAgent>();
    }


    public override State RunCurrentState()
    {
        
        if (findEnemies.enemiesInRange.Count == 0)
        {
            exclamationMark.SetActive(false);
            
            return chaseState;
        }
        else if (findEnemies.enemiesInRange[0].Item1 > this.unitStats.GetAttackRange())
        {
            exclamationMark.SetActive(false);
            
            return chaseState;
        }
        else
        {
            LookAtTarget();
            exclamationMark.SetActive(true);
            if (attackTimer >= 1.0f)
            {
                this.findEnemies.enemiesInRange[0].Item2.SendMessage("TakeDamage", this.unitStats.GetAttack());
                attackTimer = 0.0f;
            }
        }
        return this;
    }

    private void LookAtTarget()
    {
        Vector3 direction = (findEnemies.enemiesInRange[0].Item2.transform.position - agent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
    }


}
