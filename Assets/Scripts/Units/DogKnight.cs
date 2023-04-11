using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogKnight : Hero
{
    [Header("Dog Knight Settings")]
    public float qMaxDuration; // max q buff should be active for

    private bool qActive = false;
    private float qOngoing = 0; // how long has q buff been active for

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        qTimer += Time.deltaTime; // time since last cast (for tracking cooldown)

        if (qActive) 
        { 
            qOngoing += Time.deltaTime; // how long has w been active for
            
            if (qOngoing >= qMaxDuration) 
            { // buff wears off
                qActive = false;
                qOngoing = 0;

                navMeshAgent.speed /= 2; // current default
                attackDamage /= 2; // current default

            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && selected)
        {
            if (CheckQCD())
            {
                UseQ();
            }
            else
            {
                // cooldown message
                DamageNum.Create(transform.position, (qCD - qTimer).ToString("0.0") + "s left", DamageNum.colors.pink); 
            }

        }
    }

    public override void UseQ()
    {
        qTimer = 0;
        qActive = true;

        navMeshAgent.speed *= 2; // movespeed buff
        attackDamage *= 2; // attack buff
        attackTimer = attackSpeed; //reset attack timer, like garen q
        DamageNum.Create(transform.position, "Woof!", DamageNum.colors.pink); // buff message
    }
}
