﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rikayon : MonoBehaviour
{
    public float AttackCoolDown = 10.0f;
    private float attackCoolDownTimer;

    private Animator animator;

    private NavMeshAgent agent;

    private EnemyBehavior enemyBehavior;

    private Vector3 velocityPrev;

    private string currentAnimationState;

    private bool triggerAnimation;

    private GameObject currentTarget
    {
        get { return enemyBehavior.currentTarget; }
    }

    public AudioClip[] attackAudio;
    private AudioSource source;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyBehavior = GetComponent<EnemyBehavior>();
        currentAnimationState = "Rest_1";
        animator = GetComponent<Animator>();
        source = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attackCoolDownTimer > 0.0f)
            attackCoolDownTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int roll = Random.Range(1, 6);
            animator.SetTrigger(string.Format("Attack_{0}", roll));
        }

        UpdateAnimations();
    }

    #region AI
    #endregion

    void UpdateAnimations()
    {
        Vector3 velocity = agent.velocity;
        Vector3 deltaVelocity = velocityPrev - velocity;

        if (currentAnimationState != "Walk_Cycle_1" && velocity.magnitude > 0.1f)
        {
            currentAnimationState = "Walk_Cycle_1";
            triggerAnimation = true;
        }

        if (currentAnimationState == "Walk_Cycle_1" && velocity.magnitude < 0.1f)
        {
            currentAnimationState = "Rest_1";
            triggerAnimation = true;
        }

        if (currentAnimationState == "Attack" && animator.GetCurrentAnimatorStateInfo(0).IsName("Armature|Fight_Idle_1"))
            currentAnimationState = "Fight_Idle_1";

        if (triggerAnimation)
            animator.SetTrigger(currentAnimationState);


        velocityPrev = velocity;
        triggerAnimation = false;
    }

    public void Attack()
    {
        if (currentAnimationState == "Attack" || currentTarget == null || attackCoolDownTimer > 0.0f)
            return;

        currentAnimationState = "Attack";
        int randomAttackIndex = Random.Range(1, 6);
        animator.SetTrigger(string.Format("Attack_{0}", randomAttackIndex));
        attackCoolDownTimer = AttackCoolDown;

        int randomAudioIndex = Random.Range(0, 2);
        source.clip = attackAudio[randomAudioIndex];
        source.Play();
    }

    public void DoDamage(float damage)
    {
        if (currentTarget != null) currentTarget.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);
    }
}
