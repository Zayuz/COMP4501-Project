using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : Hero
{

    // Update is called once per frame
    
    protected override void Update()
    {
        base.Update();
        qTimer += Time.deltaTime; // time since last attack (for tracking cooldown)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (qTimer >= qCD)
            {
                GameObject dest = getClickedObject(out RaycastHit hit);
                destination = new Vector3(hit.point.x, hit.point.y, hit.point.z);

                transform.position = destination;
                DamageNum.Create(transform.position, "BAMF!", DamageNum.colors.pink);
                qTimer = 0;
                destination = transform.position;
            }

        }
    }


}
