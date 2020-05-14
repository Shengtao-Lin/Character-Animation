using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject agnets;

    public Text time, day;
    private Dictionary<GameObject,Vector3> startPos = new Dictionary<GameObject, Vector3>();
    private List<GameObject> all_agents = new List<GameObject>();
    private List<NavMeshAgent> all_nav = new List<NavMeshAgent>();

    private List<GameObject> eat = new List<GameObject>();
    private List<GameObject> work = new List<GameObject>();
    private List<GameObject> play = new List<GameObject>();
    bool agentGo = true;
    
    bool agentBack = true;


    public Transform eat_trans;
    public Transform play_trans;
    public Transform work_trans;

    public main main_script;

    void Start()
    {
        foreach(Transform agent in agnets.transform)
        {
            all_agents.Add(agent.gameObject);
            all_nav.Add(agent.gameObject.GetComponent<NavMeshAgent>());
            startPos.Add(agent.gameObject,agent.position);

            //Debug.Log(agent.name + " " + startPos[agent.gameObject]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(agentGo)
        {
            foreach(GameObject agent in all_agents)
            {
                NavMeshAgent nav = agent.GetComponent<NavMeshAgent>();
                //nav.destination = foo.position;

                nav.SetDestination(foo.position);
            }

            BrakeAgent();
            checkFlag();
        }

        
        if(agentBack)
        {
            Debug.Log("agent go = " + agentGo);
            foreach(GameObject agent in all_agents)
            {
                NavMeshAgent nav = agent.GetComponent<NavMeshAgent>();
                nav.isStopped = false;
                
                Vector3 dest = startPos[agent];
                dest.y = 0;
                nav.SetDestination(dest);
                
                //nav.SetDestination(foo2.position);
            }
        }
        */

        if(main_script.hour >= 20 || main_script.hour <= 8)
        {
            //go back and stay in cell
            Debug.Log("stay in cell");
            foreach(GameObject agent in all_agents)
            {
                NavMeshAgent nav = agent.GetComponent<NavMeshAgent>();
                nav.isStopped = false;
                
                Vector3 dest = startPos[agent];
                dest.y = 0;
                nav.SetDestination(dest);
                
                //nav.SetDestination(foo2.position);

                play.Clear();
                eat.Clear();
                work.Clear();
                seletctAgents();
            }

            // seletct who work, who play, who eat
            
            
        }
        else if(main_script.hour > 8)
        {
            // play
            foreach(GameObject agent in play)
            {
                NavMeshAgent nav = agent.GetComponent<NavMeshAgent>();
                //nav.destination = foo.position;

                nav.SetDestination(play_trans.position);
            }
            

            // work
            foreach(GameObject agent in work)
            {
                NavMeshAgent nav = agent.GetComponent<NavMeshAgent>();
                //nav.destination = foo.position;

                nav.SetDestination(work_trans.position);
            }

            // eat
            foreach(GameObject agent in eat)
            {
                NavMeshAgent nav = agent.GetComponent<NavMeshAgent>();
                //nav.destination = foo.position;

                nav.SetDestination(eat_trans.position);
            }
        }
        
        
    }

    // save for later
    /*
    void BrakeAgent()
    {
        float distance = 0;
        foreach(NavMeshAgent nav in all_nav)
        {
            Vector3 nav_pos = nav.transform.position;
            Vector3 foo_pos = foo.transform.position;
            nav_pos.y = 0;
            foo_pos.y = 0;

            distance += Vector3.Distance(nav_pos,foo_pos);
        }

        //Debug.Log(distance);

        if(distance/40 <= 5.0)
        {
            foreach(NavMeshAgent nav in all_nav)
            {
                nav.isStopped = true;
            }
        }
        else
        {
            foreach(NavMeshAgent nav in all_nav)
            {
                nav.isStopped = false;
            }
        }
    }

    void checkFlag()
    {
        // check if every agent is stopped, i.e. arrived at its destination

        // flag is true if every agent arrived
        Debug.Log("executing checkFlag");
        foreach(NavMeshAgent nav in all_nav)
        {
            if(nav.isStopped)
            {
                agentBack = true;
                agentGo = false;
            }
            else
            {
                agentBack = false;
                agentGo = true;
            }
        }
    }
    */

    void seletctAgents()
    {
        Dictionary<GameObject,bool> choose_list = new Dictionary<GameObject, bool>();

        foreach(GameObject agent in all_agents)
        {
            choose_list.Add(agent,false);
        }

        for(int i=0;i<12;i++)
        {
            //select 12 agents to work
            int ran = (int)Random.Range(0f,39f);
            if( choose_list[all_agents [ran] ])
            {
                ran = (int)Random.Range(0f,39f);
            }
            var agent = all_agents[ran];
            choose_list[agent] = true;
            play.Add(agent);
        }

        for(int i=0;i<12;i++)
        {
            //select 12 agents to work
            int ran = (int)Random.Range(0f,39f);
            if( choose_list[all_agents [ran] ])
            {
                ran = (int)Random.Range(0f,39f);
            }
            var agent = all_agents[ran];
            choose_list[agent] = true;
            eat.Add(agent);
        }
        
        foreach(GameObject agent in all_agents)
        {
            if(choose_list[agent] == false)
            {
                work.Add(agent);
            }
        }
    }
}
