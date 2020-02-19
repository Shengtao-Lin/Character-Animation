using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavObstController : MonoBehaviour
{

    private Transform navObstacles;

    public Material clicked;
    public Material defaultMaterial;

    private RaycastHit hitPosition;

    public KeyCode forward;
    public KeyCode backward;
    public KeyCode left;
    public KeyCode right;

    public float speed = 2f;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitPosition, 100);
            //letf click
            if(hitPosition.collider.tag == "NavObst")
            {
                if(navObstacles == null)
                {
                    navObstacles = hitPosition.collider.GetComponent<Transform>();
                    navObstacles.GetComponent<MeshRenderer>().material = clicked;
                }
                else if(navObstacles.name == hitPosition.collider.name)
                {
                    //Debug.Log("got'cha bitch!");
                    navObstacles.GetComponent<MeshRenderer>().material = defaultMaterial;
                    navObstacles = null;
                }
                else
                {
                    navObstacles.GetComponent<MeshRenderer>().material = defaultMaterial;
                    navObstacles = hitPosition.collider.GetComponent<Transform>();
                    navObstacles.GetComponent<MeshRenderer>().material = clicked;
                }
            }
        }


    }
    private void FixedUpdate() {
        //handle null navObstacles??
        if(Input.GetKey(forward))
        {
            navObstacles.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        if(Input.GetKey(backward))
        {
            navObstacles.Translate(Vector3.forward * Time.deltaTime * speed * -1);
        }
        if(Input.GetKey(left))
        {
            navObstacles.Translate(Vector3.left * Time.deltaTime * speed);
        }
        if(Input.GetKey(right))
        {
            navObstacles.Translate(Vector3.left * Time.deltaTime * speed * -1);
        }
    }
}
