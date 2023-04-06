using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Credit to https://www.youtube.com/watch?v=vAVi04mzeKk&ab_channel=SpawnCampGames for providing the framework for this function
public class DragSelect : MonoBehaviour
{ 
    Camera camera;

    [SerializeField]
    RectTransform boxVisual;

    Rect selectionBox;
    SelectionTracker selected_table;

    Vector2 startPos;
    Vector2 endPos;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        startPos = Vector2.zero;
        endPos = Vector2.zero;
        selected_table = GetComponent<SelectionTracker>();
        DrawVisual();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            selectionBox = new Rect();
        }

        if(Input.GetMouseButton(0))
        {
            endPos = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!(Input.GetKey(KeyCode.LeftShift)))
            {
                //selected_table.deselectAll();
            }
            SelectUnits();
            startPos = Vector2.zero;
            endPos = Vector2.zero;
            DrawVisual();
        }
    }

    void DrawVisual()
    {
        Vector2 boxStart = startPos;
        Vector2 boxEnd = endPos;

        Vector2 boxCenter = (boxStart + boxEnd) / 2;
        boxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        boxVisual.sizeDelta = boxSize;
    }

    void DrawSelection()
    {
        if(Input.mousePosition.x < startPos.x)
        {
            selectionBox.xMin = Input.mousePosition.x;
            selectionBox.xMax = startPos.x;
        }   
        else
        {
            selectionBox.xMin = startPos.x;
            selectionBox.xMax = Input.mousePosition.x;
        }

        if(Input.mousePosition.y < startPos.y)
        {
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = startPos.y;
        }
        else
        {
            selectionBox.yMin = startPos.y;
            selectionBox.yMax = Input.mousePosition.y;
        }
    }

    void SelectUnits()
    {
        GameObject[] all_units = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject item in all_units)
        {
            if (selectionBox.Contains(camera.WorldToScreenPoint(item.transform.position)))
            {
                Unit unit = item.GetComponent<Unit>();
                if (unit != null && unit.team == Interactable.teams.allied)
                {
                    selected_table.addSelected(item.GetComponent<Unit>());
                }
            }

        }
    }
}
