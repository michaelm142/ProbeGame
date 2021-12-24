using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : AiState 
{
    private GameObject previousTarget;

    private Vector3 targetPreviousPosition;
    private Vector3 targetVelocity;

    public Idle(EnemyBehavior behavior) : base(behavior) { }

    public override void Update()
    {
        var targets = GetTargets();
        if (targets.Count == 0)
        {
            if (previousTarget != null)
                behavior.SetCurrentState(new Alert(behavior, previousTarget.transform.position, targetVelocity));
            behavior.currentTarget = null;
            return;
        }


        targets.Sort(TargetHeruistic);
        behavior.currentTarget = targets[0];

        targetVelocity = behavior.currentTarget.transform.position - targetPreviousPosition;

        behavior.agent.SetDestination(behavior.currentTarget.transform.position);

        previousTarget = behavior.currentTarget;
        targetPreviousPosition = behavior.currentTarget.transform.position;
    }
}