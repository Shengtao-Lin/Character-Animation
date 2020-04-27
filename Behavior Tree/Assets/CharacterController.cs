using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    private RaycastHit hitPosition;

    // Update is called once per frame


    void Start(){
        
    }
    void Update()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if(Input.GetMouseButtonDown(1))
        {
            // right click: move seletced agents
            //hitPosition = 
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitPosition, 100);
            
           
                agent.destination = hitPosition.point;
            
        }
        
    }

   
}
