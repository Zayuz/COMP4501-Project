using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Minion : Unit
{   
    public int moveSpeed;
    protected UnityEngine.AI.NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    new protected virtual void Start()
    {
        base.Start();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.angularSpeed = moveSpeed;
        GameObject dest = null;

        // default clicked object is the enemy shield, hopefully will move to attack
        if (team == teams.allied)
        {
            dest = GameObject.Find("Enemy Tree");
        }
        else if (team == teams.enemy) {
            dest = GameObject.Find("Ally Tree");
        }

        if (dest != null)
        { // check if item or unit clicked on
            clickedUnit = dest.GetComponent<Unit>();
            clickedItem = dest.GetComponent<Item>();
        }
        else
        { // destination is null so no unit or item could be clicked on
            clickedUnit = null;
            clickedItem = null;

            if (GetComponent<FlockMovement>() != false)
            {
                GetComponent<FlockMovement>().seek = false;
            }
        }
    }

    // Update is called once per frame
    new protected virtual void Update()
    {
        // TODO: Make summons have different behaviour from units
        base.Update();

        navMeshAgent.destination = destination;
        if (attackTimer <= 0)
        { // attacks once then dies, will attack enemy tree
            Destroy(gameObject);
        }
    }
}
