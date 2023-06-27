using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public float speed = 3f;
    public float sprintModifier = 1.5f;
    public float rotationSpeed = 10f;

    private Animator animator;
    private Vector3 lastDirection = Vector3.zero;
    private bool isMoving = false;
    private bool isRunning = false;
    private float movementSpeed;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        movementSpeed = GetMovementSpeed();
        isMoving = IsMoving();
        isRunning = IsRunning();

        if (isMoving)
        {
            Move();
            Rotate();
            Animate();
        }
        else
        {
            SetIdle();
        }
    }

    private float GetMovementSpeed()
    {
        return Input.GetKey(KeyCode.LeftShift) ? speed * sprintModifier : speed;
    }

    private bool IsMoving()
    {
        return Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0;
    }

    private bool IsRunning()
    {
        return isMoving && Input.GetKey(KeyCode.LeftShift);
    }

    private void Move()
    {
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        inputDirection = inputDirection.normalized;
        lastDirection = inputDirection;
        transform.position += inputDirection * movementSpeed * Time.deltaTime;
    }

    private void Rotate()
    {
        Quaternion lookRotation = Quaternion.LookRotation(lastDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    private void Animate()
    {
        animator.SetBool("IsWalking", !isRunning);
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsIdle", false);
    }

    private void SetIdle()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsIdle", true);
    }
}
