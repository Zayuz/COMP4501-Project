using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : Unit
{
    public float attackSpeed;
    public float attackDamage;
    public int range;
    public int moveSpeed;
    public int defense;
    public bool selected;

    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && selected) {
            GameObject dest = getClickedObject(out RaycastHit hit);
            transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            Debug.Log(transform.position);
        }
    }

    private void OnMouseDown() { // add way to multi select and de-select
        if (Input.GetKey(KeyCode.LeftControl))
        {
            selected = false;
            Debug.Log("De-selected");
        }
        else {
            selected = true;
            Debug.Log("Selected");
        }
    }
    /*
    private Vector3 getPosUnderCursor() { 
        Vector2 screenPos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(screenPos);

        return mouseWorldPos;
    }*/

    GameObject getClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            target = hit.collider.gameObject;
        }
        return target;
    }

}
