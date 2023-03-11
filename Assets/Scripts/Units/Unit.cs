using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : Interactable
{
    [Header("Unit Settings")]
    public float maxHealth;
    public float currentHealth;
    public float attackSpeed; // in seconds
    public float attackDamage;
    public int range;
    public int defense;
    public LayerMask groundLayer;
    public HealthBar healthBar;

    protected Vector3 destination;
    protected Unit clickedUnit;
    protected Item clickedItem;
    protected float attackTimer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        attackTimer += Time.deltaTime; // time since last attack (for tracking cooldown)

        if (Input.GetMouseButtonDown(1) && selected && (team == teams.allied))
        { // only allow commanding allied units
            GameObject dest = getClickedObject(out RaycastHit hit);
            destination = new Vector3(hit.point.x, hit.point.y, hit.point.z);

            if (dest != null)
            { // check if item or unit clicked on
                clickedUnit = dest.GetComponent<Unit>();
                clickedItem = dest.GetComponent<Item>();
            }
            else
            { // destination is null so no unit or item could be clicked on
                clickedUnit = null;
                clickedItem = null;
            }
        }

        if (clickedUnit != null)
        {
            AttackTarget();
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

    GameObject getClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit, groundLayer))
        {
            target = hit.collider.gameObject;
        }

        return target;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        DamageNum.Create(transform.position, (int)damage);
        if (currentHealth <= 0 && maxHealth >= 0) {
            Destroy(gameObject);
            return;
        }

        healthBar.SetCurrentHealth(currentHealth);
        
        Debug.Log("Health: " + currentHealth);
    }

    protected void AttackTarget()
    {
        if (clickedUnit.team != this.team && clickedUnit.maxHealth != 0)
        { // walk towards target to attack until in range
            float targetDist = Mathf.Sqrt(Mathf.Pow(clickedUnit.transform.position.x - transform.position.x, 2) + Mathf.Pow(clickedUnit.transform.position.z - transform.position.z, 2));

            if ((targetDist <= range) && (attackTimer >= attackSpeed))
            {
                destination = transform.position; // stop moving to attack
                clickedUnit.TakeDamage(attackDamage);
                attackTimer = 0; // reset attack timer
            }
            // we don't set clickedUnit to null so unit continues attacking unless commanded elsewhere
        }

        if (clickedUnit == null) // check if object is destroyed
        {
            clickedUnit = null; // object needs to be manually set to null since it is not null when destroyed (weird I know)
        }
    }
}
