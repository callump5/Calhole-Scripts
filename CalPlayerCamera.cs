using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalPlayerCamera : MonoBehaviour
{
    public Transform target; // Player transform
    public float followSpeed = 5f; // Speed to follow the player
    public float rotationSpeed = 180f; // Speed to rotate the camera
    public Vector3 offset = new Vector3(0f, 10f, -10f); // Camera offset from the target

    private Transform pivot; // Pivot point for camera rotation


    private void CreatePivot()
    {
        pivot = new GameObject("CameraPivot").transform;
        pivot.position = target.position;
        pivot.rotation = Quaternion.identity;
        transform.SetParent(pivot);
        transform.localPosition = offset;
        transform.localRotation = Quaternion.Euler(80f, 0f, 0f);
    }
    
    private void Start()
    {
       CreatePivot();
    }

    private void LateUpdate()
    {
        // Follow the player smoothly
        pivot.position = Vector3.Lerp(pivot.position, target.position, Time.deltaTime * followSpeed);

        // Rotate the camera around the pivot when clicking and dragging
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            pivot.Rotate(Vector3.up, mouseX, Space.World);
        }

        // Turn the camera to look at the target
        transform.LookAt(target);
    }
}

