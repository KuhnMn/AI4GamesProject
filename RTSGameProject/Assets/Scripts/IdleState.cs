using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public PatrolState patrolState;
    public bool startPatrol;


    public override State RunCurrentState()
    {
        if (Input.GetKeyDown("p"))
        {
            startPatrol = true;
        }
        if (startPatrol)
        {
            startPatrol = false;
            return patrolState;
        }
        else
        {
            return this;
        }
    }
}
