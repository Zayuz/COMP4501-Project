using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float CameraSpeed;
    public float topBarrier;
    public float bottomBarrier;
    public float leftBarrier;
    public float rightBarrier;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.mousePosition.y >= Screen.height * topBarrier || Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.back * Time.deltaTime * CameraSpeed, Space.World);
        }
        if (Input.mousePosition.y <= Screen.height * bottomBarrier || Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(Vector3.forward * Time.deltaTime * CameraSpeed, Space.World);
        }
        if (Input.mousePosition.x >= Screen.width * leftBarrier || Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Vector3.left * Time.deltaTime * CameraSpeed, Space.World);
        }
        if (Input.mousePosition.x <= Screen.width * rightBarrier || Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Vector3.right * Time.deltaTime * CameraSpeed, Space.World);
        }
    }
}
