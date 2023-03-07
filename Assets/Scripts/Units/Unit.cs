using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public MeshCollider myMeshCollider;
    public int health;
    public enum teams { neutral, allied, enemy} // team tag
    public teams team;
    public Outline outline;

    public Outline GetOutline() { return outline; }

    public void AssignTeamOutline()
    {
        outline.enabled = false;
        if (team == teams.neutral)
        {
            outline.OutlineColor = Color.yellow;
        }
        else if (team == teams.allied)
        {
            outline.OutlineColor = Color.blue;
        }
        else if (team == teams.enemy)
        {
            outline.OutlineColor = Color.red;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
