using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Hero hero;
    public bool inCombat;
    public int combatScore;
    public int priority;

    // Start is called before the first frame update
    void Start()
    {
        combatScore = 2;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the unit is in combat and update accordingly
        CombatCheck();
        Unit self = GetComponent<Unit>();

        if (inCombat)
        {
            //Pick most significant target to attack
            if (self.clickedUnit == null) {
                float targetDist = 1000;
                //Check all enemies within 40f
                //Layermask 8 = units
                int layerMask = 1 << 8;
                Collider[] hitColliders = Physics.OverlapSphere(transform.localPosition, 40f, layerMask);
                foreach (var hitCollider in hitColliders)
                {
                    Unit unit = hitCollider.GetComponent<Unit>();
                    if (unit != null)
                    {
                        if (unit.team != self.team)
                        {
                            //Pick the closest to attack!
                            float dist = Mathf.Sqrt(Mathf.Pow(unit.transform.localPosition.x - transform.localPosition.x, 2) + Mathf.Pow(unit.transform.localPosition.z - transform.localPosition.z, 2));
                            if (dist < targetDist)
                            {
                                targetDist = dist;
                                self.clickedUnit = unit;
                            }
                        }
                    }
                }
            }

            if (combatScore >= 0 && self.clickedUnit != null)
            {
                //Priority 2, Continue victorious combat
                //Nothing changes
                self.AttackTarget();
                priority = 2;
            }
            else
            {
                //Priority 1, retreat from losing combat (or consume healing item)
                //When in combat and losing consume healing item, call allies, or run
                //Potentially call to other AI units to help in combat
                priority = 1;
                if ((self.currentHealth < self.maxHealth / 2) && hero.CheckPotions() > 0)
                {
                    hero.UsePotion();
                }
                else
                {
                    //Call for support and retreat
                    CallAllies();
                    self.destination = new Vector3(-72, 1, -2);
                    //self.clickedUnit = null;
                }
            }
        }
        else
        {
            priority = 3;
            //Priority 3, seek nearby weak enemies

            //Use collision sphere to determine enemies in range 60 and hunt them down if they are weak and we are strong
            float targetDist = 1000;
            int layerMask = 1 << 8;
            Collider[] hitColliders = Physics.OverlapSphere(transform.localPosition, 60f, layerMask);
            foreach (var hitCollider in hitColliders)
            {
                Unit unit = hitCollider.GetComponent<Unit>();
                if (unit != null)
                {
                    if (unit.team != self.team)
                    {
                        //Pick the closest to attack!
                        float dist = Mathf.Sqrt(Mathf.Pow(unit.transform.localPosition.x - transform.localPosition.x, 2) + Mathf.Pow(unit.transform.localPosition.z - transform.localPosition.z, 2));
                        //If the enemy is under half health, hunt them down
                        if (dist < targetDist && unit.currentHealth < unit.maxHealth / 2)
                        {
                            targetDist = dist;
                            self.clickedUnit = unit;
                        }
                    }
                }
            }

            //Priority 4, defend your half of the map if no enemies were found for hunting down
            if (targetDist == 1000)
            {
                GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
                foreach (GameObject gameobj in units) {
                    Unit unit = gameobj.GetComponent<Unit>();
                    if (unit != null)
                    {
                        if (unit.team != self.team)
                        {
                            //Filter for units within the area (x < -20) to defend, then pick the closest
                            float dist = Mathf.Sqrt(Mathf.Pow(unit.transform.localPosition.x - transform.localPosition.x, 2) + Mathf.Pow(unit.transform.localPosition.z - transform.localPosition.z, 2));
                            //If the enemy is under half health, hunt them down
                            if (dist < targetDist && unit.transform.localPosition.x < -20)
                            {
                                priority = 4;
                                targetDist = dist;
                                self.clickedUnit = unit;
                            }
                        }
                    }
                }
            }

            //Priority 5, acquire potions and summoning points
            //No weak enemies in range? Even enemies only and no resources? Nothing to defend? Stock up on resources
            if (targetDist == 1000)
            {
                bool needCrystal = false;
                bool needPotion = false;
                if (hero.CheckPotions() < 1)
                {
                    needPotion = true;
                }
                if (hero.CheckSummons() < 1)
                {
                    needCrystal = true;
                }
                //If you are not already stocked up on potions and crystals
                if (needPotion || needCrystal)
                {
                    GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
                    //If health potions or summmoning crystals are available
                    foreach (GameObject item in items)
                    {
                        Item.ItemType i = item.GetComponent<Item>().type;
                        if (i == Item.ItemType.Potion && needPotion)
                        {
                            float dist = Mathf.Sqrt(Mathf.Pow(item.transform.localPosition.x - transform.localPosition.x, 2) + Mathf.Pow(item.transform.localPosition.z - transform.localPosition.z, 2));
                            if (dist < targetDist)
                            {
                                priority = 5;
                                targetDist = dist;
                                self.clickedItem = item.GetComponent<Item>();
                                self.destination = item.transform.position;
                            }
                        }
                        else if (i== Item.ItemType.Crystal && needCrystal)
                        {
                            float dist = Mathf.Sqrt(Mathf.Pow(item.transform.localPosition.x - transform.localPosition.x, 2) + Mathf.Pow(item.transform.localPosition.z - transform.localPosition.z, 2));
                            if (dist < targetDist)
                            {
                                priority = 5;
                                targetDist = dist;
                                self.clickedItem = item.GetComponent<Item>();
                                self.destination = item.transform.position;
                            }
                        }
                    }
                }
            }

            //Priority 6, destroy enemy structures and strong enemies
            //When all else fails, find enemies to be destroyed
            if (targetDist == 1000)
            {
                GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
                foreach (GameObject gameobj in units)
                {
                    Unit unit = gameobj.GetComponent<Unit>();
                    if (unit != null)
                    {
                        if (unit.team != self.team)
                        {
                            float dist = Mathf.Sqrt(Mathf.Pow(unit.transform.localPosition.x - transform.localPosition.x, 2) + Mathf.Pow(unit.transform.localPosition.z - transform.localPosition.z, 2));
                            if (dist < targetDist)
                            {
                                priority = 6;
                                targetDist = dist;
                                self.clickedUnit = unit;
                                self.destination = unit.transform.position;
                            }
                        }
                    }
                }
            }
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
        Unit self = GetComponent<Unit>();
        int layerMask = 1 << 8;
        //Check all units on other teams to determine proximity

        Collider[] hitColliders = Physics.OverlapSphere(transform.localPosition, 40f, layerMask);
        foreach (var hitCollider in hitColliders)
        {
            Unit unit = hitCollider.GetComponent<Unit>();
            if (unit != null)
            {
                if (unit.team != self.team)
                {
                    targetDist = Mathf.Min(Mathf.Sqrt(Mathf.Pow(unit.transform.localPosition.x - transform.localPosition.x, 2)
                    + Mathf.Pow(unit.transform.localPosition.z - transform.localPosition.z, 2)), targetDist);
                }
            }
        }
        
        //Define in combat as in proximity to an enemy by 25 units
        if (40f > targetDist && targetDist != 100000) {
            inCombat = true;
            EvaluateCombat();
        }
        else {
            inCombat = false;
            self.clickedUnit = null;
        }
    }

    void EvaluateCombat()
    {
        //Determine likelihood to win current combat
        //Layermask 8 = units
        int layerMask = 1 << 8;
        Unit self = GetComponent<Unit>();

        //Base willingness to enter combat == 1
        combatScore = 1;

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
    }

    void CallAllies()
    {
        //Call allies to help the unit in battle if they are available and nearby
        //Check other allies in 40f if they have the enemyAI script. If they do and their priority is greater than 2, they come help.
        return;
    }
}
