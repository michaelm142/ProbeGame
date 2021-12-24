using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Alert : AiState
{
    public const float CoolDown = 10.0f;
    private float coolDownTimer;

    private Vector3 LastKnownPosition,
        LastKnownVelocity,
        randomPosition;

    private bool investigatedLastKnown;

    public Alert(EnemyBehavior behavior, Vector3 LastKnownPosition, Vector3 LastKnownVelocity) : base(behavior) 
    {
        this.LastKnownPosition = LastKnownPosition;
        this.LastKnownVelocity = LastKnownVelocity;

        coolDownTimer = CoolDown;

        randomPosition = GetRandomPosition();

    }

    private Vector3 GetRandomPosition()
    {
        float randX = Random.Range(-1.0f, 1.0f);
        float randZ = Random.Range(-1.0f, 1.0f);
        Vector3 randomOffset = new Vector3(randX, 0.0f, randZ);

        randomOffset = LastKnownPosition + randomOffset.normalized * 10.0f;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomOffset, out hit, 100.0f, NavMesh.AllAreas);

        return hit.position;
    }

    public override void Update()
    {
        coolDownTimer -= Time.deltaTime;
        if (coolDownTimer <= 0.0f)
        {

            behavior.ReturnToPreviousState();
            agent.SetDestination(transform.position);
            return;
        }

        var targets = GetTargets();
        if (targets.Count == 0)
        {
            if (!investigatedLastKnown && Vector3.Distance(behavior.transform.position, LastKnownPosition + LastKnownVelocity) > 1.0f)
            {
                agent.SetDestination(LastKnownVelocity + LastKnownPosition);
                return;
            }
            else
                investigatedLastKnown = true;

            agent.SetDestination(randomPosition);
            if (Vector3.Distance(behavior.transform.position, randomPosition) < behavior.agent.stoppingDistance)
                randomPosition = GetRandomPosition();
        }
        else
            behavior.SetCurrentState(new Idle(behavior));
    }
}
