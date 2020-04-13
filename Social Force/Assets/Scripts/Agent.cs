using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public float radius;
    public float mass;
    public float perceptionRadius;

    private List<Vector3> path;
    private NavMeshAgent nma;
    private Rigidbody rb;

    private HashSet<GameObject> perceivedNeighbors = new HashSet<GameObject>();
    private HashSet<GameObject> adjcentWalls = new HashSet<GameObject>();

    private List<GameObject> neighborEvaders = new List<GameObject>();
    private List<GameObject> neighborPursuers = new List<GameObject>();
    

    void Start()
    {
        path = new List<Vector3>();
        nma = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        gameObject.transform.localScale = new Vector3(2 * radius, 1, 2 * radius);
        nma.radius = radius;
        rb.mass = mass;
        GetComponent<SphereCollider>().radius = perceptionRadius / 2;
    }

    private void Update()
    {
        if (path.Count > 1 && Vector3.Distance(transform.position, path[0]) < 1.1f)
        {
            path.RemoveAt(0);
        } else if (path.Count == 1 && Vector3.Distance(transform.position, path[0]) < 2f)
        {
            path.RemoveAt(0);

            if (path.Count == 0)
            {
                //please comment the following two lines for spiral Force and pursue and Evade
                gameObject.SetActive(false);
                AgentManager.RemoveAgent(gameObject);
            }
        }

        #region Visualization

        if (false)
        {
            if (path.Count > 0)
            {
                Debug.DrawLine(transform.position, path[0], Color.green);
            }
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(path[i], path[i + 1], Color.yellow);
            }
        }

        if (false)
        {
            foreach (var neighbor in perceivedNeighbors)
            {
                Debug.DrawLine(transform.position, neighbor.transform.position, Color.yellow);
            }
        }

        #endregion
    }

    #region Public Functions

    public void ComputePath(Vector3 destination)
    {
        
        nma.enabled = true;
        var nmPath = new NavMeshPath();
        nma.CalculatePath(destination, nmPath);
        path = nmPath.corners.Skip(1).ToList();
        //path = new List<Vector3>() { destination };
        //nma.SetDestination(destination);
        nma.enabled = false;
        

        // leader
        /*
        if((int.Parse(name.Split(' ')[1]) == 0))
        {
            nma.enabled = true;
            var nmPath = new NavMeshPath();
            nma.CalculatePath(destination, nmPath);
            path = nmPath.corners.Skip(1).ToList();
            path = new List<Vector3>() { destination };
            nma.SetDestination(destination);
            //nma.enabled = false;
        }
        */ 
    }

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }

    #endregion

    #region Incomplete Functions

    private Vector3 ComputeForce()
    {
        var force = Vector3.zero;
        //part1 force
        //force = CalculateGoalForce(30)*0.85f+CalculateAgentForce()*0.3f+CalculateWallForce()*0.2f;

        /*
        // leader
        if((int.Parse(name.Split(' ')[1])) == 0)
            force = CalculateGoalForce(5)*0.7f+CalculateAgentForce()*0.3f+CalculateWallForce()*0.3f;
        else
            force = CaculateLeaderfollowerForce();
        */    

        //crowd follow
        //force = CrowdFollowing(60) + CalculateAgentForce()*0.3f+CalculateWallForce()*0.3f;

        //spiral Force
        //force = CalculateSpiralForce();

        //pursue and Evade
        //force = CalculatePursueEvade() + CalculateAgentForce()*0.3f;

        //Walk Wall
        //force = CalculateWallFollower();

        //Debug.DrawLine(transform.position,force,Color.red);

        if (force != Vector3.zero)
        {
            return force.normalized * Mathf.Min(force.magnitude, Parameters.maxSpeed);
        } else
        {
            return Vector3.zero;
        }
    }
    
    private Vector3 CalculateGoalForce(float maxS)
    {
        var desired_direction =Vector3.Normalize(path[0] - transform.position);
        var desired_speed = desired_direction.normalized * Mathf.Min(desired_direction.magnitude,maxS);
        var goal_force= (mass*(desired_speed-GetVelocity())/Parameters.T);

        return goal_force;
        
    }

    private Vector3 CalculateAgentForce()
    {
        var agentForce=Vector3.zero;
        var A=Parameters.A;
        var B=Parameters.B;
        var Kappa=Parameters.Kappa;
        var K=Parameters.k;

        foreach(var n in perceivedNeighbors){
            if(AgentManager.IsAgent(n)){
                var neighbor = AgentManager.agentsObjs[n];
                var pos = transform.position;
                var neighborpos =neighbor.transform.position;
                var neighborVelo=neighbor.GetVelocity();
            
                var dir = (pos-neighborpos).normalized;// vector pointing away from agent
                dir.y=0;
                var overlap = (radius+neighbor.radius)-Vector3.Distance(pos,neighborpos);//edge to edge distance
                var tangent = Vector3.Cross(Vector3.up, dir);//vector pointing tangential to agent

            
                if(overlap>0f){
                    //collision avoidance + non-penetration + sliding force
                    agentForce +=(A * Mathf.Exp(overlap / B) + K*overlap)*dir + Kappa * overlap * Vector3.Dot(rb.velocity-neighborVelo,tangent)*tangent;
                }else{
                    //if the don't touch then we dont need to calculate the non-penetration and sliding force
                    agentForce += A * Mathf.Exp(overlap / B)* dir;
                }
            }
        }

        return agentForce;
    }

    private Vector3 CalculateWallForce()
    {
        var wallForce = Vector3.zero;
        var agentForce=Vector3.zero;
        var A=Parameters.A;
        var B=Parameters.B;
        var Kappa=Parameters.Kappa;
        var K=Parameters.k;

        foreach (var wall in adjcentWalls){
            var wallpos = wall.transform.position;
            var pos = transform.position;
            var normal = (pos - wallpos).normalized; // vector pointing away from wall


            if(Mathf.Abs(normal.x)>Mathf.Abs(normal.z)){
                normal.z=0;
                normal.y=0;
            }
            else{
                normal.x=0;
                normal.y=0;
            }
            
            var overlap = (radius+0.5f)-Vector3.Distance(pos,wallpos);//edge to edge distance
            var tangent = Vector3.Cross(Vector3.up, normal);//vector pointing tangential to agent

            //g(x) part
            if(overlap>0f){
                //collision avoidance + non-penetration - sliding force
                wallForce += (A * Mathf.Exp(overlap / B)  + K * overlap)*normal - Kappa * overlap * Vector3.Dot(rb.velocity,tangent)*tangent;
            }else{
                //if the don't touch then we dont need to calculate the non-penetration and sliding force
                wallForce +=A * Mathf.Exp(overlap / B)*normal;
            }

        }

        
        return wallForce;
    }

    public void ApplyForce()
    {
        var force = ComputeForce();
        force.y = 0;
        //defult force for part1 and crowd following
        rb.AddForce(force * 10, ForceMode.Force);
        // growing spriral
        //rb.AddForce(force * 10, ForceMode.Force);

        // eavder and pursuer
        //rb.AddRelativeForce(force * 10,ForceMode.Force);

        // Leader
        /*
        if((int.Parse(name.Split(' ')[1])) == 0)
            rb.AddForce(force * 20, ForceMode.Force);
        else
            rb.AddRelativeForce(force * 5,ForceMode.Force);
        */
    }

    private Vector3 CalculateSpiralForce()
    {
        var spiralForce = Vector3.zero;
        var centerDir = Vector3.zero - transform.position;
        
        if(centerDir.magnitude > 0)
        {
            spiralForce += Vector3.Cross(Vector3.up,centerDir).normalized * 0.01f;
            spiralForce += centerDir.normalized * 0.0075f;
        }
        
        return spiralForce;
    }

    public Vector3 CrowdFollowing(float maxS){
        var CrowdForce = Vector3.zero;
        var panicParameter=0.8f;
        var desired_direction =Vector3.Normalize(path[0] - transform.position);
        var avgDir=Vector3.zero;

        foreach(var n in perceivedNeighbors){
            if(AgentManager.IsAgent(n)){
                var neighbor = AgentManager.agentsObjs[n];
                var neighbordir =neighbor.GetComponent<Rigidbody>().velocity;
                //Debug.DrawLine(neighbor.transform.position,neighbordir,Color.red);
                avgDir += neighbordir;
            }
        }
        avgDir=avgDir/perceivedNeighbors.Count;
        var ezero= ((1-panicParameter)*desired_direction+ panicParameter*avgDir.normalized).normalized;


        var desired_speed = Mathf.Min(desired_direction.magnitude,maxS) * ezero;
        CrowdForce= (mass*(desired_speed-GetVelocity())/Parameters.T);

        return CrowdForce;  
    }

    private Vector3 CalculatePursueEvade()
    {
        var agentForce = Vector3.zero;
        bool isEvader = (int.Parse(name.Split(' ')[1]) % 2) == 0;
        
        if(isEvader)
        {
            // I am Evader
            Debug.DrawLine(transform.position,transform.position + Vector3.up * 3, Color.green, 0.1f);
            GetComponent<SphereCollider>().radius = 5f;

            foreach(var n in perceivedNeighbors.Where(n => AgentManager.IsAgent(n)))
            {
                Agent neighbor = AgentManager.agentsObjs[n];
                var dir = (transform.position - neighbor.transform.position).normalized;

                var overlap = (radius + neighbor.radius) - Vector3.Distance(transform.position,n.transform.position);

                var otherIsEvader = (int.Parse(n.name.Split(' ')[1]) %2 == 0);
                if(otherIsEvader)
                {
                    agentForce += Mathf.Exp(overlap) * dir * 0.1f;
                }
                
                
                else
                {
                    agentForce += dir * 0.1f;
                    var tangent = Vector3.Cross(Vector3.up,dir);
                    agentForce += tangent;
                }
                
            }
                       
        }
        else
        {
            // I am Pursuer
            Debug.DrawLine(transform.position,transform.position + Vector3.up * 3, Color.red, 0.1f);

            if(neighborEvaders.Count == 0)
            {
                GetComponent<SphereCollider>().radius *= 1.5f;
            }
            else
            {
                GameObject evader_gameObj= neighborEvaders[0];
                Transform trans= evader_gameObj.GetComponent<Transform>();
                Agent evader = AgentManager.agentsObjs[evader_gameObj];
                transform.LookAt(evader.transform);
                Vector3 dir = Vector3.forward * 0.5f;
                
                agentForce += dir;
                
            }
        }
        
        
        return agentForce;
    }

    private Vector3 CaculateLeaderfollowerForce()
    {
        var leaderForce = Vector3.zero;
        Agent leader = AgentManager.agents[0];
        if((int.Parse(name.Split(' ')[1])) == 0)
        {
            return leaderForce;
        }
        else
        {
            if(Vector3.Distance(leader.transform.position,transform.position) < 1f)
            {
                return leaderForce;
            }
        }

        transform.LookAt(leader.transform);
        Vector3 dir = Vector3.forward * 0.1f;
        leaderForce += dir;

        return leaderForce;
    }
    public void OnTriggerEnter(Collider other)
    {
        if(AgentManager.IsAgent(other.gameObject)){
            perceivedNeighbors.Add(other.gameObject);

            bool isEvader = (int.Parse(other.name.Split(' ')[1]) % 2) == 0;
            if(isEvader)    neighborEvaders.Add(other.gameObject);
            else    neighborPursuers.Add(other.gameObject);
        }
        if(WallManager.IsWall(other.gameObject)){
            adjcentWalls.Add(other.gameObject);
        }
    }
    
    public void OnTriggerExit(Collider other)
    {
        if(perceivedNeighbors.Contains(other.gameObject)){
            perceivedNeighbors.Remove(other.gameObject);

            bool isEvader = (int.Parse(other.name.Split(' ')[1]) % 2) == 0;
            if(isEvader)    neighborEvaders.Remove(other.gameObject);
            else    neighborPursuers.Remove(other.gameObject);
        }
        if(adjcentWalls.Contains(other.gameObject)){
            adjcentWalls.Remove(other.gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {

    }

    public void OnCollisionExit(Collision collision)
    {
        
    }
    #endregion
}
