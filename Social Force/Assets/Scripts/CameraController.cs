using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    public KeyCode forward;
    public KeyCode backward;
    public KeyCode left;
    public KeyCode right;
    public KeyCode up;
    public KeyCode down;

    public float camera_moveSpeed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        //todo: left & right-click 

        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        if(Input.GetKey(forward))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * camera_moveSpeed);
        }
        if(Input.GetKey(backward))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * camera_moveSpeed * -1);
        }
        if(Input.GetKey(left))
        {
            transform.Translate(Vector3.right * Time.deltaTime * camera_moveSpeed * -1);
        }
        if(Input.GetKey(right))
        {
            transform.Translate(Vector3.right * Time.deltaTime * camera_moveSpeed);
        }
        if(Input.GetKey(up))
        {
            transform.Translate(Vector3.up * Time.deltaTime * camera_moveSpeed, Space.World);
        }
        if(Input.GetKey(down))
        {
            transform.Translate(Vector3.up * Time.deltaTime * camera_moveSpeed * -1, Space.World);
        }
        if (Input.GetAxis ("Mouse ScrollWheel") > 0) 
        {
            if (Camera.main.fieldOfView >= 20) 
            {
                Camera.main.fieldOfView -= 5;
            }
        }
        if (Input.GetAxis ("Mouse ScrollWheel") < 0) 
        {
            if (Camera.main.fieldOfView <= 50) 
            {
                Camera.main.fieldOfView += 5;
            }
        }

    }
}