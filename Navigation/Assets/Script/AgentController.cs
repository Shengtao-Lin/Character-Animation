using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    public Material clicked;
    
    public LayerMask groundLayer;

    List<NavMeshAgent> agents = new List<NavMeshAgent>();
    List<int> priority = new List<int>();
    private int rand = 99;

    private RaycastHit hitPosition;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // left click: select an agent
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitPosition, 100);
            if(hitPosition.collider.tag == "Agent")
            {
                MeshRenderer temp_render = hitPosition.collider.GetComponent<MeshRenderer>();
                NavMeshAgent temp_agent = hitPosition.collider.GetComponent<NavMeshAgent>();

                if(!agents.Contains(temp_agent))
                {
                    while(priority.Contains(rand))
                    {
                        rand = (int)Random.Range(0,99);
                        //Debug.Log("rand = " + rand);
                    }
                    priority.Add(rand);
                    temp_agent.avoidancePriority = rand;
                    temp_render.material = clicked;
                    agents.Add(temp_agent);
                }
                else
                {
                    Debug.Log("removing " + temp_agent.avoidancePriority);
                    priority.Remove(temp_agent.avoidancePriority);
                    temp_render.material = new Material(Shader.Find("Diffuse"));
                    agents.Remove(temp_agent);
                }
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            // right click: move seletced agents
            hitPosition = getPointUnderCursor();
            
            foreach(NavMeshAgent agent in agents)
            {
                agent.destination = hitPosition.point;
            }
        }
    }

    
    private RaycastHit getPointUnderCursor()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitPosition, 100, groundLayer);
        return hitPosition;
    }
    
}
