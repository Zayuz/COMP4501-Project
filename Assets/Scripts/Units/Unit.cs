using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Unit : Interactable
{
    [Header("Unit Settings")]
    public float maxHealth;
    public float currentHealth;
    public float attackSpeed; // in seconds
    public float attackDamage;
    public int range;
    public int defense;
    public HealthBar healthBar;
    public Vector3 destination;
    public Unit clickedUnit;
    public Item clickedItem;

    protected float attackTimer;
    protected Animator animator;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        attackTimer = attackSpeed;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
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
                
                if (GetComponent<FlockMovement>() != null)
                {
                    GetComponent<FlockMovement>().seek = true;
                }
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
        
        if (GetComponent<FlockMovement>() != null)
        {
            if (destDist <= 10.0)
            {
                //Change behavior in flocks to reflect arrival at destination
                GetComponent<FlockMovement>().seek = false;
            }
        }
    }

    public void Select()
    {
        GetOutline().OutlineColor = Color.white;
        selected = true;
        GetOutline().enabled = true;
    }

    public void Deselect()
    {
        selected = false;
        GetOutline().enabled = false;
    }

    public void TakeDamage(float damage)
    {
        // calculate damage with defense taken into account
        damage *= 1f - ((0.052f * defense) / (0.9f + 0.048f * defense));
        damage = (float)Math.Ceiling(damage);
        currentHealth -= damage;

        // Different colours for allied vs enemy damage for visibility
        if (damage <= 0)
        {
            if (team == teams.allied)
            {
                DamageNum.Create(transform.position, ((int)(damage)).ToString(), DamageNum.colors.red);
            }
            else
            {
                DamageNum.Create(transform.position, ((int)(damage)).ToString(), DamageNum.colors.orange);
            }
        }

        if (currentHealth <= 0 && maxHealth >= 0) 
        {
            if (this is TreeStructure)
            {
                if (team == teams.enemy)
                {
                    // win the game
                    SceneManager.LoadScene("Victory");
                }
                else
                {
                    // lose the game
                    SceneManager.LoadScene("Defeat");
                }
            }

            Destroy(gameObject);
            return;
        }

        if (animator != null)
        {
            animator.SetBool("isTakingDamage", true);
        }

        healthBar.SetCurrentHealth(currentHealth);
    }

    public void Heal(float hp) 
    {
        float amountHealed = hp;

        if ((currentHealth + hp) > maxHealth)
        {
            amountHealed = maxHealth - currentHealth;
            currentHealth = maxHealth;
        }
        else 
        {
            currentHealth += hp;
        }

        DamageNum.Create(transform.position, ((int)amountHealed).ToString(), DamageNum.colors.green);

        healthBar.SetCurrentHealth(currentHealth);
    }

    public void AttackTarget()
    {
        if (clickedUnit.team != this.team && clickedUnit.maxHealth != 0)
        { // walk towards target to attack until in range
            float targetDist = Mathf.Sqrt(Mathf.Pow(clickedUnit.transform.position.x - transform.position.x, 2) + Mathf.Pow(clickedUnit.transform.position.z - transform.position.z, 2));

            if ((targetDist <= range) && (attackTimer >= attackSpeed))
            {
                destination = transform.position; // stop moving to attack
                clickedUnit.TakeDamage(attackDamage);

                attackTimer = 0; // reset attack timer

                if (GetComponent<FlockMovement>() != null)
                {
                    //Change behavior in flocks to reflect arrival at destination
                    GetComponent<FlockMovement>().seek = false;
                }

                if (animator != null)
                {
                    animator.SetBool("isMoving", false);
                    animator.SetBool("isAttacking", true);
                    animator.SetBool("isTakingDamage", false);
                }
            }
            else if (targetDist > range)
            {
                destination = clickedUnit.transform.position;

                if (animator != null)
                {
                    animator.SetBool("isMoving", true);
                    animator.SetBool("isAttacking", false);
                    animator.SetBool("isTakingDamage", false);
                }
            }
            // we don't set clickedUnit to null so unit continues attacking unless commanded elsewhere
        }
    }
}
