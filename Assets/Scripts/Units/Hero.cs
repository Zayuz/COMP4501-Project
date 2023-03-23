using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Unit
{
    public int moveSpeed;

    private int potions;
    private int summoningPoints;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    protected float qTimer; // timer for ability on q key
    public float qCD; // cooldown in seconds

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        qTimer = qCD;
        potions = 0;
        summoningPoints = 0;

        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.angularSpeed = moveSpeed;
        destination = transform.position;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        if (clickedItem != null)
        {
            PickUpItem();
        }

        if (Input.GetKeyDown(KeyCode.F) && potions > 0 && selected) { // use potion
            if (currentHealth >= maxHealth)
            {
                Debug.Log("Already at max health!");
            }
            else {
                Heal(100);
                potions--;
            }
        }

        if (Input.GetKeyDown(KeyCode.G) && selected)
        { // take a point of damage, for testing animations and combat
            TakeDamage(1.0f);
        }

        navMeshAgent.destination = destination;
    }

    void PickUpItem()
    { // walk towards item to pick up until in range
        float targetDist = Mathf.Sqrt((float)Mathf.Pow(clickedItem.transform.position.x - transform.position.x, 2) + Mathf.Pow(clickedItem.transform.position.z - transform.position.z, 2));

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
}
