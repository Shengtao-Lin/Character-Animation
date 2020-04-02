using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
[RequireComponent (typeof (Animator))]
public class PlayerController_room : MonoBehaviour
{
    Animator anim;
    NavMeshAgent agent;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator> ();
        agent = GetComponent<NavMeshAgent> ();
        agent.updatePosition = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;
        float dx = Vector3.Dot (transform.right, worldDeltaPosition);
        float dy = Vector3.Dot (transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2 (dx, dy);
        

        float smooth = Mathf.Min(1.0f, Time.deltaTime/0.15f);
        
        smoothDeltaPosition = Vector2.Lerp (smoothDeltaPosition, deltaPosition, 0.7f);

        

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f){
            velocity = smoothDeltaPosition / Time.deltaTime;
            
        }
        bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;
        
        
       if(shouldMove){
           anim.SetFloat ("Vertical", 1f);
       }else if(velocity.x>0.1){
           anim.SetFloat ("Vertical", velocity.x);
       }else{
           anim.SetFloat ("Vertical", 0f);
       }

       if(agent.isOnOffMeshLink){
           anim.SetBool ("isJump", true);
       }else{
           anim.SetBool ("isJump", false);
       }
        
        anim.SetFloat ("Horizontal", velocity.y);
    }

    void OnAnimatorMove ()
    {
 

        transform.position = agent.nextPosition;
    }
}
