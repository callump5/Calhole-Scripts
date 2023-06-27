using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementScript : MonoBehaviour
{
    public Transform goal;
    public Vector3 followOffset;
    
    private Animator animator;
    

    void Start() {
        animator = GetComponent<Animator>();
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.stoppingDistance = 0.1f; // Set a small stopping distance
    }

    void Update() {
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.SetDestination(goal.position + followOffset);

        if (agent.remainingDistance <= agent.stoppingDistance) {
            animator.SetBool("IsIdle", true);
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsWalking", false);
        } else {
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsWalking", false);
        }
    }
}
