using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardSelect : MonoBehaviour
{
    //Credit to https://www.youtube.com/watch?v=OL1QgwaDsqo&ab_channel=Seabass for providing the framework for this function
    SelectionTracker selected_table;
    RaycastHit hit;
    public LayerMask UnitLayer;

    Vector3 p1;
    static Texture2D _whiteTexture;

    // Start is called before the first frame update
    void Start()
    {
        selected_table = GetComponent<SelectionTracker>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, UnitLayer))
            {
                Unit unit = hit.transform.GetComponent<Unit>();
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if(unit != null && unit.team == Interactable.teams.allied)
                    {
                        selected_table.addSelected(unit);
                    }
                }
                else if (unit != null && unit.team == Interactable.teams.allied)
                {
                    selected_table.deselectAll();
                    selected_table.addSelected(unit);
                }
                else
                {
                    selected_table.deselectAll();
                }
            }
            else //if we didnt hit something
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    //do nothing
                }
                else
                {
                    selected_table.deselectAll();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Unit unit = hit.transform.GetComponent<Unit>();
        if (unit != null && unit.team == Interactable.teams.allied)
        {
            selected_table.addSelected(unit);
        }
    }
}
