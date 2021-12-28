using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : AiState 
{
    private GameObject previousTarget;

    public Idle(EnemyBehavior behavior) : base(behavior) { }

    public override void Update()
    {
        var targets = GetTargets(); 
        if (Vector3.Distance(transform.position, behavior.startPosition) > agent.stoppingDistance)
            agent.SetDestination(behavior.startPosition);

        if (targets.Count > 0)
        {
            targets.Sort(TargetHeruistic);
            behavior.PushCurrentState(new Attack(behavior, targets[0]));
        }
    }
}