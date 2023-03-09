using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : Interactable
{
    public float attackSpeed;
    public float attackDamage;
    public int range;
    public int moveSpeed;
    public int defense;
    public Vector3 destination;
    public LayerMask groundLayer;
    public Unit clickedUnit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0 && maxHealth >= 0) {
            Destroy(gameObject);
            return;
        }
        if (Input.GetMouseButtonDown(1) && selected) 
        {
            GameObject dest = getClickedObject(out RaycastHit hit);
            destination = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            if (dest) { // null reference check
                
                clickedUnit = dest.GetComponent<Unit>();
                if (clickedUnit != null && clickedUnit.team != teams.allied && clickedUnit.maxHealth != 0)
                { // attack
                    float targetDist = (float)Math.Sqrt((float)Math.Pow(hit.point.x - transform.position.x, 2) + (float)Math.Pow(hit.point.y - transform.position.y, 2)); // distance calc
                    Debug.Log(targetDist);
                    if (targetDist <= range)
                    {
                        destination = transform.position; // stop to attack
                        clickedUnit.currentHealth -= attackDamage;
                        Debug.Log("Attack Made");
                    }
                }
            }
            
        }

        Vector3 unitDirection = (destination - transform.position);
        unitDirection = unitDirection.normalized;

        transform.position = transform.position + (unitDirection * moveSpeed) * Time.deltaTime;
        if (Vector3.Distance(transform.position, destination) < 0.1f) { // stop vibrating position if close enough
            destination = transform.position;
        }
    }

    private void OnMouseDown() // add way to multi select and de-select
    { 
        if (Input.GetKey(KeyCode.LeftControl) && selected == true)
        {
            selected = false;
            GetOutline().enabled = false;
        }
        else if (selected == false && team == teams.allied) 
        {
            GetOutline().OutlineColor = Color.white;
            selected = true;
            GetOutline().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        destination = transform.position;
        Debug.Log("COLLISION");
    }

    GameObject getClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit, groundLayer))
        {
            target = hit.collider.gameObject;
        }

        return target;
    }
}
