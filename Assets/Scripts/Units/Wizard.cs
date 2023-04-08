using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wizard : Hero
{
    [Header("Wizard Settings")]
    public float healPotency;
    public float healRange;

    private bool qActive = false;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        qTimer += Time.deltaTime; // time since last cast (for tracking cooldown)

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

        if (clickedUnit != null && qActive)
        {
            HealTarget();
        }
    }

    public override void UseQ()
    {
        GameObject dest = getClickedObject(out RaycastHit hit);

        if (dest != null)
        { // check if item or unit clicked on
            clickedUnit = dest.GetComponent<Unit>();
            destination = new Vector3(hit.point.x, hit.point.y, hit.point.z);

            if (clickedUnit)
            {
                qActive = true;
            }
        }
        else
        {
            clickedUnit = null;
            clickedItem = null;
            qActive = false;
        }
    }

    public void HealTarget()
    {
        if (clickedUnit != null && clickedUnit.team == this.team)
        {
            float targetDist = Mathf.Sqrt(Mathf.Pow(clickedUnit.transform.position.x - transform.position.x, 2) + Mathf.Pow(clickedUnit.transform.position.z - transform.position.z, 2));

            if (targetDist <= healRange)
            {
                destination = transform.position; // stop moving to attack
                clickedUnit.Heal(healPotency);
                qTimer = 0; // reset timer
                qActive = false;
                // NOTE: add healing animation here if we want
            }
        }
    }
}
