using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.AI;

public class MobileUnit : MonoBehaviour {

	NavMeshAgent agent;
    List<Transform> waypoints;
    [SerializeField] float speed = 1;
    int waypointIndex;
    bool isMoving = false;

    public List<Transform> Waypoints
    {
        set
        {
            waypoints = value;
            waypointIndex = 0;
            agent.SetDestination(waypoints[waypointIndex].position);
            isMoving = true;
        }
    }

	void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		Assert.IsNotNull(agent);	
        agent.speed = speed;
	}

    void Update() 
    {
        if (isMoving && !agent.pathPending && agent.remainingDistance < 0.8f)
        {
                GotoNextPoint();
        }
    }

    void GotoNextPoint()
    {
        if(waypointIndex < waypoints.Count -1)
        {
            waypointIndex++;
            agent.SetDestination(waypoints[waypointIndex].position);
        }
        else
        {
            isMoving = false;
            agent.ResetPath();
        }
    }
    
    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
            agent.speed = value;
        }
    }
}
