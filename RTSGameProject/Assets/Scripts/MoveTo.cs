using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    public Transform goal;
    public float speed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.destination = goal.position;
    }

    // Update is called once per frame
    void Update(){
    }
}
