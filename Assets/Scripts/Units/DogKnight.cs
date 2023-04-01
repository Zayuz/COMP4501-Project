using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogKnight : Hero
{
    private bool qActive = false;
    private float qOngoing = 0; // how long has q buff been active for
    public float qMaxDuration; // max q buff should be active for

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        qTimer += Time.deltaTime; // time since last cast (for tracking cooldown)
        if (qActive) { 
            qOngoing += Time.deltaTime; // how long has w been active for
            if (qOngoing >= qMaxDuration) { // buff wears off
                //Debug.Log("BUFF OVER");
                qActive = false;
                qOngoing = 0;

                navMeshAgent.speed = 10; // current default
                attackDamage = 20; // current default

            }
        }



        if (Input.GetKeyDown(KeyCode.Q) && selected)
        {
            if (qTimer >= qCD)
            {
                qTimer = 0;
                qActive = true;

                navMeshAgent.speed = 20; // movespeed buff
                attackDamage = 50; // attack buff
                attackTimer = attackSpeed; //reset attack timer, like garen q
                DamageNum.Create(transform.position, "Woof!", DamageNum.colors.pink); // buff message
            }
            else
            {
                DamageNum.Create(transform.position, (qCD - qTimer).ToString("0.0") + "s left", DamageNum.colors.orange); // cooldown message
            }

        }
    }
}
