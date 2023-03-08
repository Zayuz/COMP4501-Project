using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public MeshCollider myMeshCollider;
    public int health;
    public enum teams { neutral, allied, enemy } // team tag
    public teams team;
    public Outline outline;

    protected bool selected;

    void Awake()
    {
        GetOutline().enabled = false;
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Outline GetOutline() { return outline; }

    public void AssignTeamOutline()
    {
        GetOutline().enabled = false;
        
        if (team == teams.neutral)
        {
            GetOutline().OutlineColor = Color.yellow;
        }
        else if (team == teams.allied)
        {
            GetOutline().OutlineColor = Color.cyan;
        }
        else if (team == teams.enemy)
        {
            GetOutline().OutlineColor = Color.red;
        }
    }

    private void OnMouseOver() 
    {
        if (!selected)
        {
            GetOutline().OutlineColor = Color.white;
            GetOutline().enabled = true;
        }
    }

    private void OnMouseExit() 
    {
        if (!selected)
        {
            GetOutline().enabled = false;
        }
    }
}
