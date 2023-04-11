using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Unit
{
    public int moveSpeed;
    public float qCD; // cooldown in seconds
    public GameObject summon; // object to summon when using "summon" ability

    private int potions;
    private int summoningPoints;
    protected UnityEngine.AI.NavMeshAgent navMeshAgent;
    protected float qTimer; // timer for ability on q key
    public ShieldStructure myShield;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        qTimer = qCD; // abilities available from the start
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

        /*if (Input.GetKeyDown(KeyCode.E) && selected && summon != null)
        {
            Summon();
        }*/

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

    public int CheckSummons()
    {
        return summoningPoints;
    }

    public void UsePotion()
    {
        Heal(100);
        potions--;
        return;
    }

    public void Summon()
    {
        if (summoningPoints > 0)
        {
            float offset = 3f;
            Vector3 position = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
            Instantiate(summon, position, transform.rotation, transform).SetActive(true);
            summoningPoints--;
        }
        else
        {
            DamageNum.Create(transform.position, "Out of summons", DamageNum.colors.pink);
        }
    }
}
