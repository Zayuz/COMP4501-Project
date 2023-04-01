using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Hero hero;
    public bool inCombat;
    public bool winningCombat;

    // Start is called before the first frame update
    void Start()
    {
        inCombat = false;
        winningCombat = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the unit is in combat and update accordingly
        CombatCheck();

        if (inCombat)
        {
            //Priority 1, retreat from losing combat (or consume healing item)
            //When in combat and enemy attacker is healthier than the self by 20%, consume healing item or run
            //Potentially call to other AI units to help in combat

            //Priority 2, continue victorious combat
            //Nothiing changes
        }
        else
        {
            //Priority 3, seek nearby weak enemies
            //Use collision sphere to determine enemies in range 40 and hunt them down if they are weak and we are strong

            //Priority 4, acquire potions and summoning points
            //No enemies in 40 range? Even enemies only and no resources? Stock up on things

            //Priority 5, destroy enemy structures and strong enemies
            //When all else fails, find enemies to be destroyed
        }

        //When in combat, use Q ability if able for additional power
        if (inCombat)
        {
            if (hero.CheckQCD())
            {
                hero.UseQ();
            }
        }
    }

    void CombatCheck()
    {
        float targetDist = 100000;
        //Check all units on team 'allied' to determine if they are nearby
        GameObject[] all_units = GameObject.FindGameObjectsWithTag("Unit");
        Interactable.teams team = GetComponent<Unit>().team;
        foreach (var item in all_units)
        {
            Unit unit = item.GetComponent<Unit>();
            if (unit != null)
            {
                if(unit.team != team)
                {
                    targetDist = Mathf.Min(Mathf.Sqrt(Mathf.Pow(unit.transform.localPosition.x - transform.localPosition.x, 2)
                    + Mathf.Pow(unit.transform.localPosition.z - transform.localPosition.z, 2)), targetDist);
                }
            }
        }

        //Define in combat as in proximity to an enemy by 25 units
        if (25f >= targetDist) {
            inCombat = true;
            EvaluateCombat();
        }
        else {
            inCombat = false;
        }
    }

    void EvaluateCombat()
    {
        //Determine likelihood to win current combat
        //Layermask 8 = units
        int layerMask = 1 << 8;
        Unit self = GetComponent<Unit>();

        //Base willingness to enter combat == 2
        int combatScore = 2;

        //Consider health of unit before anything else
        if (self.currentHealth <= 3 * self.maxHealth / 4)
        {
            combatScore--;
        }
        if (self.currentHealth <= self.maxHealth / 2)
        {
            combatScore--;
        }
        if (self.currentHealth <= self.maxHealth / 4)
        {
            combatScore--;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.localPosition, 40f, layerMask);
        foreach (var hitCollider in hitColliders)
        {
            Unit unit = hitCollider.GetComponent<Unit>();
            if (unit != null)
            {
                //Detect if outnumbered or if backed up by allies
                if(unit.team == self.team)
                {
                    combatScore++;
                }
                else
                {
                    combatScore--;
                    //Weaker foes means more motive to stay in combat!
                    if(unit.currentHealth <= 3 * unit.maxHealth / 4)
                    {
                        combatScore++;
                    }
                    if(unit.currentHealth <= unit.maxHealth / 2)
                    {
                        combatScore++;
                    }
                    if(unit.currentHealth <= unit.maxHealth / 4)
                    {
                        combatScore++;
                    }
                }
            }
        }

        //Debug.Log(combatScore);

        if(combatScore >= 0)
        {
            winningCombat = true;
        }
        else
        {
            winningCombat = false;
        }
    }
}
