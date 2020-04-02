using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{

    
    public LayerMask groundLayer;

    List<NavMeshAgent> agents = new List<NavMeshAgent>();
    List<NavMeshAgent> agentsrun = new List<NavMeshAgent>();
    List<NavMeshAgent> allagents = new List<NavMeshAgent>();
    List<Animator> anims=new List<Animator>();

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

                Animator temp_anim = agentHitPosition.collider.GetComponent<Animator>();


                
                
                if(!agents.Contains(temp_agent)&&!agentsrun.Contains(temp_agent))
                {
                    
                    temp_agent.speed=1.0f;
                    agents.Add(temp_agent);
                    allagents.Add(temp_agent);
                    
                    
                }
                else if(agents.Contains(temp_agent))
                {
                    //temp_render.material = new Material(Shader.Find("Diffuse"));
                    agents.Remove(temp_agent);
                    temp_agent.speed=2.0f;
                    agentsrun.Add(temp_agent);
                    anims.Add(temp_anim);
                    
                }else if(agentsrun.Contains(temp_agent)){
                    agentsrun.Remove(temp_agent);
                    anims.Remove(temp_anim);
                    temp_anim.SetBool ("isShift", false);
                    allagents.Remove(temp_agent);
                }
                    
               
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            // right click: move seletced agents
            //hitPosition = 
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitPosition, 100, groundLayer);
            foreach(Animator aim in anims){
                aim.SetBool ("isShift", true);
            }
            foreach(NavMeshAgent agent in allagents)
            {
                agent.destination = hitPosition.point;
            }
        }
        BreakAgent();
    }

    private void BreakAgent(){
        distance=0;
        foreach(NavMeshAgent x in allagents){
            Vector3 a=x.transform.position;
            Vector3 d=hitPosition.point;
            a.y = 0;
            d.y=0;
            distance+=Vector3.Distance(a,d);
            if(Vector3.Distance(a,d)<.5){
                x.isStopped = true;
            }
        }
        //print(distance/agents.Count);
        if(distance/allagents.Count<=1.3){
            foreach(NavMeshAgent x in allagents){
                x.isStopped = true;
                //print("xxxxx");
            }
        }
        
        if(distance/allagents.Count>1.3){
            foreach(NavMeshAgent x in allagents){
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
        yield return new WaitForSeconds(.5f); 
    }
    




}
