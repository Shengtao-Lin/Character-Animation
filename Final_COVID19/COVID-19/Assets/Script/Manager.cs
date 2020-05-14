using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject agnets;
    private Dictionary<GameObject,Vector3> startPos = new Dictionary<GameObject, Vector3>();
    private List<GameObject> all_agents = new List<GameObject>();
    private List<NavMeshAgent> all_nav = new List<NavMeshAgent>();

    private List<GameObject> work = new List<GameObject>();
    private List<GameObject> play = new List<GameObject>();



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
        foreach (var item in all_nav)
        {
            item.speed=15;
            item.acceleration=13;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(main_script.hour > 20 || main_script.hour < 8)
        {
            //go back and stay in cell
            //Debug.Log("stay in cell");
            if(main_script.hour > 20 && main_script.hour <= 23)
            {
                main_script.scale = 5;
            }
            else
            {
                main_script.scale = 100;
            }
            foreach(GameObject agent in all_agents)
            {
                NavMeshAgent nav = agent.GetComponent<NavMeshAgent>();
                nav.isStopped = false;
                
                Vector3 dest = startPos[agent];
                dest.y = 0;
                nav.SetDestination(dest);
            }

            // seletct who work, who play, who eat

            play.Clear();
            work.Clear();
            seletctAgents();
        }

        else if(main_script.hour >= 8 && main_script.hour <= 13)
        {
            // all work
            main_script.scale = 10;
            foreach(GameObject agent in all_agents)
            {
                NavMeshAgent nav = agent.GetComponent<NavMeshAgent>();
                nav.isStopped = false;
                nav.SetDestination(work_trans.position);
            }
        }

        else if(main_script.hour > 13 && main_script.hour <= 16)
        {
            // all eat
            main_script.scale = 10;
            foreach(GameObject agent in all_agents)
            {
                NavMeshAgent nav = agent.GetComponent<NavMeshAgent>();
                nav.isStopped = false;
                nav.SetDestination(eat_trans.position);
            }
        }
        else
        {
            // some play some work
            main_script.scale = 10;
            foreach(GameObject agent in play)
            {
                NavMeshAgent nav = agent.GetComponent<NavMeshAgent>();
                nav.isStopped = false;
                nav.SetDestination(play_trans.position);
            }

            foreach(GameObject agent in work)
            {
                NavMeshAgent nav = agent.GetComponent<NavMeshAgent>();
                nav.isStopped = false;
                nav.SetDestination(work_trans.position);
            }

        }
        
        
    }

    void seletctAgents()
    {
        Dictionary<GameObject,bool> choose_list = new Dictionary<GameObject, bool>();

        foreach(GameObject agent in all_agents)
        {
            choose_list.Add(agent,false);
        }

        for(int i=0;i<20;i++)
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

        
        foreach(GameObject agent in all_agents)
        {
            if(choose_list[agent] == false)
            {
                work.Add(agent);
            }
        }
    }
}
