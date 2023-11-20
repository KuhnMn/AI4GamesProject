using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public ChaseState chaseState;
    public FindEnemies findEnemies;
    public UnityEngine.AI.NavMeshAgent navMeshAgent;
    public GameObject agent;

    private float attackTimer;
    private GameObject exclamationMark;

    void Start()
    {
        this.exclamationMark = agent.transform.Find("Exclamation Mark").gameObject;
    }


    public override State RunCurrentState()
    {
        if (findEnemies.enemiesInRange.Count == 0)
        {
            exclamationMark.SetActive(false);
            return chaseState;
        }
        else if (findEnemies.enemiesInRange[0].Item1 > 1.0f)
        {
            exclamationMark.SetActive(false);
            return chaseState;
        }
        else
        {
            exclamationMark.SetActive(true);
            if (attackTimer >= 1.0f)
            {
                chaseState.findEnemies.enemiesInRange[0].Item2.SendMessage("TakeDamage", agent.GetComponent<UnitStats>().GetAttack());
                attackTimer = 0.0f;
            }
        }
        return this;
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
    }


}
