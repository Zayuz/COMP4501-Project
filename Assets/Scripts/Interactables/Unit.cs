using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : Interactable
{
    [Header("Unit Settings")]
    public float attackSpeed;
    public float attackDamage;
    public int range;
    public int moveSpeed;
    public int defense;
    public LayerMask groundLayer;

    private Vector3 destination;
    private Unit clickedUnit;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Item clickedItem;
    private int potions;
    private int summoningPoints;

    // Start is called before the first frame update
    void Start()
    {
        potions = 0;
        summoningPoints = 0;
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.angularSpeed = moveSpeed;
        destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0 && maxHealth >= 0) {
            Destroy(gameObject);
            return;
        }

        if (Input.GetMouseButtonDown(1) && selected)
        {
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

        if (clickedUnit != null && clickedUnit.team != teams.allied && clickedUnit.maxHealth != 0)
        { // walk towards target to attack until in range
            float targetDist = (float)Math.Sqrt((float)Math.Pow(clickedUnit.transform.position.x - transform.position.x, 2) + (float)Math.Pow(clickedUnit.transform.position.z - transform.position.z, 2));

            if (targetDist <= range)
            {
                destination = transform.position; // stop to attack
                clickedUnit.currentHealth -= attackDamage;
                Debug.Log("Attack Made");
                clickedUnit = null;
            }
        }
        else if (clickedItem != null)
        { // walk towards target to pick up until in range
            float targetDist = (float)Math.Sqrt((float)Math.Pow(clickedItem.transform.position.x - transform.position.x, 2) + (float)Math.Pow(clickedItem.transform.position.z - transform.position.z, 2));

            if (targetDist <= 10)
            {
                destination = transform.position;
                Item.ItemType i = clickedItem.PickedUp();

                if (i == Item.ItemType.Potion)
                {
                    potions++;
                    Debug.Log("Potions: " + potions);
                }
                else if (i == Item.ItemType.Crystal)
                {
                    summoningPoints++;
                    Debug.Log("Summoning Points: " + summoningPoints);
                }

                clickedItem = null;
            }
        }

        /*Vector3 unitDirection = (destination - transform.position);
        unitDirection = unitDirection.normalized;

        transform.position = transform.position + (unitDirection * moveSpeed) * Time.deltaTime;
        if (Vector3.Distance(transform.position, destination) < 0.1f) { // stop vibrating position if close enough
            destination = transform.position;
        }*/
        navMeshAgent.destination = destination;
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

    private void OnTriggerEnter(Collider other)
    {
        //destination = transform.position;
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
}
