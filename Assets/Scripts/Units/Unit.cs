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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && selected) 
        {
            GameObject dest = getClickedObject(out RaycastHit hit);
            destination = new Vector3(hit.point.x, hit.point.y+0.5f, hit.point.z);
        }

        Vector3 unitDirection = (destination - transform.position);
        unitDirection = unitDirection.normalized;

        transform.position = transform.position + (unitDirection * moveSpeed) * Time.deltaTime;
    }

    private void OnMouseDown() // add way to multi select and de-select
    { 
        if (Input.GetKey(KeyCode.LeftControl) && selected == true)
        {
            selected = false;
            GetOutline().enabled = false;
        }
        else if (selected == false) 
        {
            AssignTeamOutline();
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
