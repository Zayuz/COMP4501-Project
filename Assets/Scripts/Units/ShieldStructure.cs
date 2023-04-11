using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class ShieldStructure : Structure
{
    public Structure tree; // tree to buff defense of
    public int defenseBuff; // what to buff tree defense by
    private float summonTimer;
    public GameObject summon;
    public float offset;
    public int summoningCrystals;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        if (tree != null)
        {
            tree.defense += defenseBuff;
        }

        summonTimer = 0f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        summonTimer -= Time.deltaTime;

        if (summonTimer <= 0f && summoningCrystals > 0)
        {
            spawnMinion();
            summonTimer = 10f; // summon minion every 10 seconds
            summoningCrystals--;
            
            if (summoningCrystals <= 0) 
            {
                DamageNum.Create(transform.position, "Out of summoning crystals!", DamageNum.colors.red);
            }
        }
    }

    public void OnDestroy()
    {
        if (tree != null)
        {
            tree.defense -= defenseBuff;
        }
    }

    public void spawnMinion()
    {
        Debug.Log("Spawning Minion!");
        Vector3 position = new Vector3(transform.position.x, transform.position.y - 14f, transform.position.z - offset);
        Instantiate(summon, position, transform.rotation, transform).SetActive(true);
    }

    public int GetCrystals()
    {
        return summoningCrystals;
    }
}

