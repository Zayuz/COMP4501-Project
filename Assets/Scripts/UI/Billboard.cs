using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform cam;

    // Update is called once per frame
    void LateUpdate()
    {
        if (cam == null) {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        }
        transform.LookAt(transform.position + cam.forward);
    }
}
