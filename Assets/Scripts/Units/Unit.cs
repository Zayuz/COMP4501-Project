using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public MeshCollider myMeshCollider;
    public int health;
    public enum teams { neutral, allied, enemy} // team tag
    public teams team;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
