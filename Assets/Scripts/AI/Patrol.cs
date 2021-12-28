using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class Patrol : AiState
{
    public const float PatrolIncrement = 10.0f;
    private float patrolDistance;

    private Vector3 targetPosition;

    private GameObject testSphere;

    private WaypointCircuit waypointCircuit;

    public Patrol(EnemyBehavior behavior) : base(behavior)
    {
        waypointCircuit = GameObject.FindObjectOfType<WaypointCircuit>();
        UpdatePosition();
    }

    public override void Update()
    {
        var targets = GetTargets();
        if (targets.Count != 0)
        {
            targets.Sort(TargetHeruistic);
            behavior.PushCurrentState(new Attack(behavior, targets[0]));
            return;
        }
        if (Vector3.Distance(transform.position, agent.destination) < agent.stoppingDistance)
        {
            patrolDistance += PatrolIncrement;
            if (patrolDistance >= 2000)
                patrolDistance = 0;
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        targetPosition = waypointCircuit.GetRoutePosition(patrolDistance);
        agent.SetDestination(targetPosition);
    }
}
