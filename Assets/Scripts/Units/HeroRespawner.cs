using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRespawner : MonoBehaviour
{
    public GameObject prefab;
    public float respawnTime;
    
    private Vector3 respawnPosition;
    private float timer;

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
            Instantiate(prefab, respawnPosition, Quaternion.identity);
            this.enabled = false;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public void SetRespawnPosition(Vector3 pos)
    {
        respawnPosition = pos;
    }

    public void StartTimer()
    {
        timer = respawnTime;
        this.enabled = true;
    }
}
