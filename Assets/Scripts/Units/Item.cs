using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public enum ItemType {
        Potion,
        Crystal
    }

    [Header("Item Settings")]
    public ItemType type;

    [Header("Respawn Settings")]
    public float respawnTime;
    public ItemRespawner respawner;
  
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ItemType PickedUp()
    {
        // Set item inactive and send to respawner
        GetOutline().enabled = false;
        respawner.StartTimer(this.gameObject, respawnTime);
        gameObject.SetActive(false);

        return type;
    }
}
