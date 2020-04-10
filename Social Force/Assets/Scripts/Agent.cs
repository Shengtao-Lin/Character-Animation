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
    //private List<Tuple<double,double>> locations = new List<Tuple<double,double>>();

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
        path = new List<Vector3>() { destination };
        nma.SetDestination(destination);
        //nma.enabled = false;
    }

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }

    #endregion

    #region Incomplete Functions

    private Vector3 ComputeForce()
    {
        var force = CalculateGoalForce(5)*0.5f+CalculateAgentForce()*0.2f+CalculateWallForce()*0.3f;

        Debug.DrawLine(transform.position,force,Color.red);

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
        var desired_direction =Vector3.Normalize(nma.destination - transform.position);
        var desired_speed = desired_direction.normalized * Mathf.Min(desired_direction.magnitude,maxS);
        var goal_force= (mass*(desired_speed-GetVelocity())/Parameters.T);

        return goal_force;
        
    }

    private Vector3 CalculateAgentForce()
    {
        var agentForce=Vector3.zero;

        foreach(var n in perceivedNeighbors){
            if(!AgentManager.IsAgent(n)){
                continue;
            }

            var neighbor = AgentManager.agentsObjs[n];
            var dir = (transform.position-neighbor.transform.position).normalized;
            var overlap = (radius+neighbor.radius)-Vector3.Distance(transform.position,n.transform.position);

            agentForce += Parameters.A * Mathf.Exp(overlap / Parameters.B)* dir;
            agentForce += Parameters.k * (overlap >0f ? 1:0) * dir;

            var tangent = Vector3.Cross(Vector3.up, dir);
            agentForce += Parameters.Kappa * (overlap>0f ? overlap :0) * Vector3.Dot(rb.velocity-neighbor.GetVelocity(),tangent)*tangent;

        }


        

        return agentForce;
    }

    private Vector3 CalculateWallForce()
    {
        var wallForce = Vector3.zero;

        foreach (var wall in adjcentWalls){
            var wallCentroid = wall.transform.position;
            var pos = transform.position;
            var normal = pos - wallCentroid;
            normal.y=0;

            if(Mathf.Abs(normal.x)>Mathf.Abs(normal.z)){
                normal.z=0;
            }
            else{
                normal.x=0;
            }
            normal.Normalize();

            var dir = pos-wallCentroid;
            dir.y=0;

            var agentWallProj = Vector3.Project(dir, normal);
            var overlap = (radius+0.5f)-agentWallProj.magnitude;

            wallForce +=Parameters.A * Mathf.Exp(overlap / Parameters.B) * normal;
            wallForce +=Parameters.k * (overlap>0f ? 1:0)*dir;

            //var tangent = Vector3.Cross(Vector3.up, normal);
            //wallForce += tangent;

        }

        
        return wallForce;
    }

    public void ApplyForce()
    {
        var force = ComputeForce();
        force.y = 0;

        rb.AddForce(force * 10, ForceMode.Force);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(AgentManager.IsAgent(other.gameObject)){
            perceivedNeighbors.Add(other.gameObject);
        }
        if(WallManager.IsWall(other.gameObject)){
            adjcentWalls.Add(other.gameObject);
        }
    }
    
    public void OnTriggerExit(Collider other)
    {
        if(perceivedNeighbors.Contains(other.gameObject)){
            perceivedNeighbors.Remove(other.gameObject);
        }
        if(adjcentWalls.Contains(other.gameObject)){
            adjcentWalls.Remove(other.gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        /*
        if(WallManager.IsWall(collision.gameObject)){
            adjcentWalls.Add(collision.gameObject);

            var wallCentroid = collision.gameObject.transform.position;
            var pos = transform.position;
            //var c = collision.contacts[0].point;
            var normal = pos-wallCentroid;
        }*/
    }

    public void OnCollisionExit(Collision collision)
    {
        
    }


    #endregion
}
