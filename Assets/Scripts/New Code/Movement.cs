using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public int moveSpeed;
    
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private FlockMovement flocking;
    private Vector3 destination;
    private Interactable thisUnit;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        flocking = GetComponent<FlockMovement>();
        thisUnit = GetComponent<Interactable>();

        navMeshAgent.angularSpeed = moveSpeed;
        destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        bool isSelected = thisUnit ? thisUnit.GetSelected() : false;

        if (Input.GetMouseButtonDown(1) && isSelected)
        { // only allow commanding if unit is + can be selected
            GameObject dest = thisUnit.getClickedObject(out RaycastHit hit);

            if (dest != null)
            { // check if item or unit clicked on
                destination = new Vector3(hit.point.x, hit.point.y, hit.point.z);

                if (flocking)
                {
                    flocking.seek = true;
                }
            }
            else
            { // destination is null
                if (flocking)
                {
                    flocking.seek = false;
                }
            }
        }

        navMeshAgent.destination = destination;
    }

    // call when action is made (attack, item, etc.) to stop moving
    public void StopMoving() 
    {
        destination = transform.position;

        if (flocking)
        {
            // change behavior in flocks to reflect arrival at destination
            flocking.seek = false;
        }
    }
}
