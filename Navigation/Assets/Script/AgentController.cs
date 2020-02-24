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

    private float distance=99999;

    private RaycastHit hitPosition;

    private Rigidbody rb;


    // Update is called once per frame


    void Start(){
        rb = GetComponent<Rigidbody>();
    }
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
        BreakAgent();
    }

    
    private RaycastHit getPointUnderCursor()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitPosition, 100, groundLayer);
        return hitPosition;
    }

    private void BreakAgent(){
        distance=0;
        foreach(NavMeshAgent x in agents){
            Vector3 a=x.transform.position;
            Vector3 d=hitPosition.point;
            a.y = 0;
            d.y=0;
            distance+=Vector3.Distance(a,d);
        }
        //print(distance/agents.Count);
        if(distance/agents.Count<=1.0){
            foreach(NavMeshAgent x in agents){
                x.isStopped = true;
                //print("xxxxx");
            }
        }else{
            foreach(NavMeshAgent x in agents){
                x.isStopped = false;
                
            }
        }
    }

    void OnTriggerEnter(Collider other){
        Debug.Log("Collision Detected "+other.gameObject.name);
        float dis1=Vector3.Distance(other.transform.position,hitPosition.point);
        float dis2=Vector3.Distance(rb.position,hitPosition.point);
        print("xxx");
        if(dis1<dis2){
            rb.Sleep();
            StartCoroutine(efekt());
            rb.WakeUp();
            
        }

    }
     IEnumerator efekt ()
    { 
        yield return new WaitForSeconds(2f); 
    }
    
}
