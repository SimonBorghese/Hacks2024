using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class BaseEnemy : MonoBehaviour
{
    public NavMeshAgent PlayerAgent;

    public Transform DebugPlayerPosition;

    public List<GameObject> Nodes;

    public Random RandomSelector;

    // The current time we've waited at a node
    public float CurrentWait;
    
    // The target wait time
    public float TargetWait;
    
    // The minimum distance from a node to be counted
    public float MinDistance;

    public enum EnemyStates
    {
        Walking,
        Waiting
    };

    public EnemyStates State;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Nodes = new List<GameObject>();
        RandomSelector = new Random();
        GameObject.FindGameObjectsWithTag("NavNode", Nodes);

        PlayerAgent.SetDestination(Nodes[0].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
        // Select the current state and do logic
        switch (State)
        {
            case EnemyStates.Walking:
                if ((transform.position - PlayerAgent.destination).magnitude < MinDistance)
                {
                    CurrentWait = 0.0f;
                    TargetWait = 5.0f;
                    State = EnemyStates.Waiting;
                }
                break;
            case EnemyStates.Waiting:
                CurrentWait += Time.deltaTime;
                if (CurrentWait >= TargetWait)
                {
                    PlayerAgent.SetDestination(Nodes[RandomSelector.Next(0, Nodes.Count)].transform.position);
                    State = EnemyStates.Walking;
                }
                break;
        }
    }
}
