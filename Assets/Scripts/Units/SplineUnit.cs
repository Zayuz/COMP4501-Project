using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SplineUnit : Unit
{
    // Start is called before the first frame update
    protected override void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        attackTimer = attackSpeed;
        animator = GetComponent<Animator>();
        destination = transform.position;
    }

    // Update is called once per frame
    protected override void Update()
    {
        attackTimer += Time.deltaTime; // time since last attack (for tracking cooldown)

        if (Input.GetMouseButtonDown(1) && selected && (team == teams.allied))
        { // only allow commanding allied units
            GameObject dest = getClickedObject(out RaycastHit hit);

            if (dest != null)
            { // check if item or unit clicked on
                clickedUnit = dest.GetComponent<Unit>();
                clickedItem = dest.GetComponent<Item>();
                destination = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                
                if (GetComponent<SplineMovement>() != null)
                {
                    GetComponent<SplineMovement>().StopMovement();
                    GetComponent<SplineMovement>().StartMovement(this.transform.position, 
                                                                destination, 
                                                                (this.transform.rotation * Vector3.forward), 
                                                                (this.transform.position - destination).normalized);
                }
            }
            else
            { // destination is null so no unit or item could be clicked on
                clickedUnit = null;
                clickedItem = null;

                if (GetComponent<SplineMovement>() != null)
                {
                    GetComponent<SplineMovement>().StopMovement();
                }
            }
        }

        // Attack target if unit is not null
        if (clickedUnit != null)
        {
            AttackTarget();
        }
        else if (animator != null)
        {
            animator.SetBool("isAttacking", false);
        }

        float destDist = Mathf.Sqrt(Mathf.Pow(destination.x - transform.position.x, 2) + Mathf.Pow(destination.z - transform.position.z, 2));

        if (animator != null) 
        {
            animator.SetBool("isTakingDamage", false);

            if (destDist >= 2.0)
            {
                animator.SetBool("isMoving", true);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isTakingDamage", false);
            }
            else 
            {
                animator.SetBool("isMoving", false);
            }
        }
    }

    new protected void AttackTarget()
    {
        if (clickedUnit.team != this.team && clickedUnit.maxHealth != 0)
        { // walk towards target to attack until in range
            float targetDist = Mathf.Sqrt(Mathf.Pow(clickedUnit.transform.position.x - transform.position.x, 2) + Mathf.Pow(clickedUnit.transform.position.z - transform.position.z, 2));

            if ((targetDist <= range) && (attackTimer >= attackSpeed))
            {
                destination = transform.position; // stop moving to attack
                clickedUnit.TakeDamage(attackDamage);

                attackTimer = 0; // reset attack timer

                if (GetComponent<SplineMovement>() != null)
                {
                    GetComponent<SplineMovement>().StopMovement();
                }

                if (animator != null)
                {
                    animator.SetBool("isMoving", false);
                    animator.SetBool("isAttacking", true);
                    animator.SetBool("isTakingDamage", false);
                }
            }
            // we don't set clickedUnit to null so unit continues attacking unless commanded elsewhere
        }
    }
}
