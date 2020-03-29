using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator anim;

    float vertical;
    float horizontal;

    bool isShift;
    bool isJump;

    bool InMyState;

    float y_pos;
    void Start()
    {
        anim = GetComponent<Animator>();
        isShift = false;
        isJump = false;

        y_pos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        anim.SetFloat("Vertical",vertical);

        horizontal = Input.GetAxis("Horizontal");
        anim.SetFloat("Horizontal",horizontal);
        
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            isShift = true;
            anim.SetBool("isShift",isShift);
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift)){
            isShift = false;
            anim.SetBool("isShift",isShift);
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            isJump = true;
            anim.SetBool("isJump",isJump);
        }
        else if(Input.GetKeyUp(KeyCode.Space)){
            isJump = false;
            anim.SetBool("isJump",isJump);
        }
        if(transform.position.y < 0 ){
            transform.position = new Vector3(transform.position.x,0f,transform.position.z);
        }    
        
    }

    private void OnCollisionStay(Collision other) {
        if(other.collider.tag == "Step"){
            transform.position = new Vector3(transform.position.x,2*other.transform.position.y,transform.position.z);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.collider.name == "Plane"){
            transform.position = new Vector3(transform.position.x,0f,transform.position.z);
        }
    }

    void OnCollisionExit(Collision other) {
        if(other.collider.tag == "Step"){
            transform.position = new Vector3(transform.position.x,0f,transform.position.z);
        }
    }
}