using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public Transform cameraTransform;
    public float speed = 3f;
    public float sprintModifier = 1.5f;
    public float rotationSpeed = 10f;

    private Animator animator;
    private UnityEngine.AI.NavMeshAgent agent;

    private bool isMoving = false;
    private bool isRunning = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void Update()
    {
        // Movement Input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        isMoving = horizontalInput != 0f || verticalInput != 0f;
        isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

        // Calculate movement speed
        float movementSpeed = isRunning ? speed * sprintModifier : speed;

        // Calculate movement direction
        Vector3 movementDirection = cameraTransform.forward * verticalInput + cameraTransform.right * horizontalInput;
        movementDirection.y = 0f; // Exclude vertical movement
        movementDirection.Normalize();

        // Move the character
        agent.Move(movementDirection * Time.deltaTime * movementSpeed);

        // Rotate the character
        if (isMoving)
        {
            Quaternion lookRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        // Update animation states
        animator.SetBool("IsWalking", isMoving && !isRunning);
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsIdle", !isMoving);
    }
}
