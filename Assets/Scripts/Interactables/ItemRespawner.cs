using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRespawner : MonoBehaviour
{
    private float timer; // respawn timer
    private GameObject targetObject;

    // Start is called before the first frame update
    void Start()
    {
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0f)
        {
            targetObject.SetActive(true);
            Debug.Log("respawned");
            this.enabled = false;
        }
        else
        {
            timer -= Time.deltaTime;
            Debug.Log("Timer: " + timer);
        }
    }

    public void StartTimer(GameObject target, float respawnTime)
    {
        targetObject = target;
        timer = respawnTime;
        this.enabled = true;
    }
}
