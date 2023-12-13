using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScoutState : State
{
    public ChaseState chaseState;
    public PatrolState patrolState;
    public Vector3 scoutLocation;
    public float scoutingRadius = 15.0f;

    public GameObject agent;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private FindEnemies findEnemies;
    private Vector3 specificScoutLocation;


    void Start()
    {
        this.findEnemies = agent.GetComponent<FindEnemies>();
        this.navMeshAgent = agent.GetComponent<NavMeshAgent>();
    }

    public override State RunCurrentState()
    {
        if (specificScoutLocation == Vector3.zero && findEnemies.enemiesInRange.Count == 0)
        {
            if (specificScoutLocation == Vector3.zero)
            {
                specificScoutLocation = GetRandomScoutPosition();
            }
            navMeshAgent.SetDestination(specificScoutLocation);
            return this;
        }
        else if (specificScoutLocation != Vector3.zero && Vector3.Distance(agent.transform.position, specificScoutLocation) < 1)
        {
            specificScoutLocation = Vector3.zero;
            return patrolState;
        }
        else if (findEnemies.enemiesInRange.Count > 0)
        {
            specificScoutLocation = Vector3.zero;
            return chaseState;
        }
        else
        {
            navMeshAgent.SetDestination(specificScoutLocation);
            return this;
        }
    }

    private Vector3 GetRandomScoutPosition()
    {
        float randomAngle = Random.Range(0, 360); // Generate a random angle in degrees
        float randomRadius = Random.Range(0, scoutingRadius); // Generate a random distance within the scouting radius

        // Convert polar coordinates to Cartesian coordinates
        float x = scoutLocation.x + randomRadius * Mathf.Cos(Mathf.Deg2Rad * randomAngle);
        float z = scoutLocation.z + randomRadius * Mathf.Sin(Mathf.Deg2Rad * randomAngle);

        return new Vector3(x, scoutLocation.y, z);
    }
}
