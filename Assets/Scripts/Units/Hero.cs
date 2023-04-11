using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Unit
{
    public int moveSpeed;
    public float qCD; // cooldown in seconds
    public ShieldStructure myShield;

    protected int potions;
    protected int summoningPoints;
    protected UnityEngine.AI.NavMeshAgent navMeshAgent;
    protected HeroRespawner respawner;
    protected float qTimer; // timer for ability on q key

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        qTimer = qCD; // abilities available from the start
        potions = 0;

        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.angularSpeed = moveSpeed;
        destination = transform.position;

        if (team == teams.allied)
        {
            myShield = GameObject.Find("Ally Shield").GetComponent<ShieldStructure>();
        }
        else
        {
            myShield = GameObject.Find("Enemy Shield").GetComponent<ShieldStructure>();
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        if (clickedItem != null)
        {
            PickUpItem();
        }

        if (Input.GetKeyDown(KeyCode.F) && selected) 
        { // use potion
            if (currentHealth >= maxHealth)
            {
                Debug.Log("Already at max health!");
            }
            else if (potions <= 0)
            {
                DamageNum.Create(transform.position, "Out of potions", DamageNum.colors.pink);
            }
            else 
            {
                UsePotion();
            }
        }
        
        if (navMeshAgent == null)
        {
            navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        }
        else
        {
            navMeshAgent.destination = destination;
        }
    }

    void OnDestroy()
    {
        if (respawner != null)
        {
            respawner.StartTimer();
        }
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
                myShield.summoningCrystals += 5;
                DamageNum.Create(transform.position, "+5 Summoning Crystals!", DamageNum.colors.pink);
            }
            else if (i == Item.ItemType.MegaMush)
            {
                Heal(maxHealth);
                attackDamage *= 2;
                attackSpeed /= 2;
                defense *= 2;
            }

            clickedItem = null;
        }
    }

    public bool CheckQCD()
    {
        return qTimer >= qCD;
    }

    public virtual void UseQ()
    {
        return;
    }

    public int CheckPotions()
    {
        return potions;
    }

    public void UsePotion()
    {
        Heal(100);
        potions--;
        return;
    }
}
