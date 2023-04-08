using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldStructure : Structure
{
    public Structure tree; // tree to buff defense of
    public int defenseBuff; // what to buff tree defense by

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        if (tree != null)
        {
            tree.defense += defenseBuff;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void OnDestroy()
    {
        if (tree != null)
        {
            tree.defense -= defenseBuff;
        }
    }
}
