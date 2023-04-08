using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : Hero
{
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
                DamageNum.Create(transform.position, (qCD-qTimer).ToString("0.0") + "s left", DamageNum.colors.pink); // cooldown message
            }
        }
    }

    public override void UseQ()
    {
        GameObject dest = getClickedObject(out RaycastHit hit);
        destination = new Vector3(hit.point.x, hit.point.y, hit.point.z);

        transform.position = destination;
        DamageNum.Create(transform.position, "BAMF!", DamageNum.colors.pink); // teleport sound effect / indicator
        qTimer = 0;
        destination = transform.position;
    }
}
