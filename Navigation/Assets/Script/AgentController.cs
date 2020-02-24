using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    public Material clicked;
    
    public LayerMask groundLayer;

    List<NavMeshAgent> agents = new List<NavMeshAgent>();

    private float distance=99999;

    private RaycastHit agentHitPosition;
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
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out agentHitPosition, 100);
            if(agentHitPosition.collider.tag == "Agent")
            {
                MeshRenderer temp_render = agentHitPosition.collider.GetComponent<MeshRenderer>();
                NavMeshAgent temp_agent = agentHitPosition.collider.GetComponent<NavMeshAgent>();

                if(!agents.Contains(temp_agent))
                {
                    temp_render.material = clicked;
                    agents.Add(temp_agent);
                }
                else
                {
                    temp_render.material = new Material(Shader.Find("Diffuse"));
                    agents.Remove(temp_agent);
                }
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            // right click: move seletced agents
            //hitPosition = 
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitPosition, 100, groundLayer);
            
            foreach(NavMeshAgent agent in agents)
            {
                agent.destination = hitPosition.point;
            }
        }
        BreakAgent();
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
        //print("xxx");
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
