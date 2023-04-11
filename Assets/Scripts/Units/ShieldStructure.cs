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
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        if (tree != null)
        {
            tree.defense += defenseBuff;
        }

        summonTimer = 10f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        summonTimer -= Time.deltaTime;

        if (summonTimer <= 0f)
        {
            spawnMinion();

            summonTimer = 10f; // summon minion every 10 seconds
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
}

