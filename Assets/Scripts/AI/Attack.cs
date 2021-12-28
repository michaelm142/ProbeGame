using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Attack : AiState
{

    private Vector3 targetPreviousPosition;
    private Vector3 targetVelocity;
    private GameObject target;

    bool targetLost;

    public Attack(EnemyBehavior behavior, GameObject target) : base(behavior)
    {
        this.target = target;
    }

    public override void Update()
    {
        if (targetLost)
        {
            behavior.ReturnToPreviousState();
            return;
        }

        var targets = GetTargets();
        if (!targets.Contains(target))
        {
            behavior.PushCurrentState(new Alert(behavior, targetPreviousPosition, targetVelocity));
            behavior.currentTarget = null;
            targetLost = true;
            return;
        }

        behavior.currentTarget = target;

        targetVelocity = behavior.currentTarget.transform.position - targetPreviousPosition;

        behavior.agent.SetDestination(behavior.currentTarget.transform.position);

        targetPreviousPosition = behavior.currentTarget.transform.position;
    }
}
