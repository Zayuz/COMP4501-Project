using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Unit
{
    public GameObject megaMush;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    private void OnDestroy()
    {
        megaMush.SetActive(true);
    }
}
