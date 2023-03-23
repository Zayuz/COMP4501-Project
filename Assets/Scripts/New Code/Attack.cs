using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackSpeed; // in seconds
    public float attackDamage;
    public int range;

    private Interactable thisUnit;
    private Interactable clickedUnit;
    private float attackTimer;

    // Start is called before the first frame update
    void Start()
    {
        thisUnit = GetComponent<Interactable>();
        attackTimer = attackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime; // time since last attack (for tracking cooldown)
        bool isSelected = thisUnit ? thisUnit.GetSelected() : false;
        
        if (Input.GetMouseButtonDown(1) && isSelected)
        {
            GameObject dest = thisUnit.getClickedObject(out RaycastHit hit);

            if (dest != null)
            { // check if unit clicked on
                clickedUnit = dest.GetComponent<Unit>();
            }
            else
            { // destination is null so no unitcould be clicked on
                clickedUnit = null;
            }
        }

        if (clickedUnit != null)
        {
            AttackTarget();
        }
    }

    public void AttackTarget()
    {
        if ((clickedUnit.GetTeam() != thisUnit.GetTeam()) && 
            (clickedUnit.GetComponent<Health>().GetCurrentHealth() != 0))
        { // only attack when in range
            float targetDist = Mathf.Sqrt(Mathf.Pow(clickedUnit.transform.position.x - transform.position.x, 2) + Mathf.Pow(clickedUnit.transform.position.z - transform.position.z, 2));

            if ((targetDist <= range) && (attackTimer >= attackSpeed))
            {
                clickedUnit.GetComponent<Health>().TakeDamage(attackDamage);
                attackTimer = 0; // reset attack timer
                
                // Stop movement if unit can move
                Movement move = GetComponent<Movement>();
                if (move)
                {
                    move.StopMoving();
                }
            }
            // we don't set clickedUnit to null so unit continues attacking unless commanded elsewhere
        }

        if (clickedUnit == null) // check if object is destroyed
        {
            clickedUnit = null; // object needs to be manually set to null since it is not null when destroyed (weird I know)
        }
    }
}
