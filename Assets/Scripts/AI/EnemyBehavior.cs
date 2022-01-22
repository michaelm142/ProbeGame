using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public float LOS = 10.0f;
    public float AttackRadius = 1.0f;
    public float SightAngle = 60.0f;

    public NavMeshAgent agent { get; private set; }

    public Vector3 startPosition { get; private set; }

    public GameObject currentTarget;

    private AiState currentState;

    public enum StartingState
    {
        Idle,
        Patrol,
    }

    public StartingState startingState;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = AttackRadius;

        if (startingState == StartingState.Idle)
            currentState = new Idle(this);
        else
            currentState = new Patrol(this);

        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Pause.Paused)
        {
            if (!agent.isStopped)
                agent.isStopped = true;
        }
        else
        {
            if (agent.isStopped)
                agent.isStopped = false;
            currentState.Update();
        }
        //Debug.Log(string.Format("Agent {0} current state: {1}", gameObject.name, currentState));
        if (currentTarget != null && Vector3.Distance(currentTarget.transform.position, transform.position) < AttackRadius)
            gameObject.BroadcastMessage("Attack", SendMessageOptions.DontRequireReceiver);
    }

    public void PushCurrentState(AiState state)
    {
        state.previous = currentState;
        currentState = state;
    }

    public void ReturnToPreviousState()
    {
        currentState = currentState.previous;
        if (currentState == null)
            currentState = new Idle(this);
    }

    public void OnDrawGizmos()
    {
        //Vector3 rightPoint = transform.position + transform.right;
        //Vector3 leftPoint = transform.position - transform.right;
        //Debug.DrawLine(rightPoint, rightPoint + transform.forward * LOS);
        //Debug.DrawLine(rightPoint + transform.forward * LOS, leftPoint + transform.forward * LOS);
        //Debug.DrawLine(leftPoint + transform.forward * LOS, leftPoint);
    }
}

public abstract class AiState
{
    protected Transform transform { get { return gameObject.transform; } }

    protected GameObject gameObject;

    public AiState previous;
    public EnemyBehavior behavior { get; private set; }

    protected NavMeshAgent agent { get; private set; }
    public AiState(EnemyBehavior behavior)
    {
        this.behavior = behavior;
        gameObject = behavior.gameObject;
        agent = behavior.GetComponent<NavMeshAgent>();
    }

    public abstract void Update();

    protected virtual int TargetHeruistic(GameObject targetA, GameObject targetB)
    {
        int distA = (int)Vector3.Distance(transform.position, targetA.transform.position);
        int distB = (int)Vector3.Distance(transform.position, targetB.transform.position);

        if (distA < distB)
            return -1;

        return 1;
    }

    protected virtual List<GameObject> GetTargets()
    {
        List<GameObject> outval = new List<GameObject>();
        var probes = GameObject.FindGameObjectsWithTag("Player");

        foreach (var p in probes)
        {
            float distance = Vector3.Distance(p.transform.position, transform.position);
            if (distance < behavior.LOS)
            {
                Vector3 L = (p.transform.position - transform.position).normalized;
                if (Vector3.Angle(L, transform.forward) < behavior.SightAngle || distance < behavior.LOS * 0.1f)
                    outval.Add(p);
            }
        }

        return outval;
    }
}
