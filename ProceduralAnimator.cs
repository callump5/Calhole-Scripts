using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralAnimator : MonoBehaviour {

    // This will be the object to move
    [SerializeField] private GameObject _rootObject;
    // Add the target of each limb
    [SerializeField] private Transform[] _limbTargets;
    // Offset for the 'step' size
    [SerializeField] public Vector3 _targetOffset = new Vector3(0f, 0f, 0.6f); 
    // Movement speed
    [SerializeField] public float _movementSpeed = 5f;


    private int _nLimbs;
    private bool _isMoving;
    private ProceduralLimb[] _limbs;

    void Start()
    {
        _nLimbs = _limbTargets.Length;
        _limbs = new ProceduralLimb[_nLimbs];

        Transform t;

        for (int i = 0; i < _nLimbs; i++)
        {
            t = _limbTargets[i];
            _limbs[i] = new ProceduralLimb()
            {
                IKTarget = t,
                defaultPosition = t.localPosition,
                lastPosition = t.position,
                isMoving = false,
                sphereObject = null
            };
        }
    }

    void FixedUpdate()
    {

        // TODO
        // 1) Calculate point to step to
        // 2) Extend Leg
        // 3) Move the body to the extended point, 
        // 4) Make the legs loop to mimic moving forward
        // 5) Raise/lower legs in their stride so it looks realistic 

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        _isMoving = horizontalInput != 0f || verticalInput != 0f;

        if (_isMoving) 
            Move();
        else 
            // DestroyTargets();
            SetToRest();
    }

    void Move()
    {
        for (int i = 0; i < _nLimbs; i++)
        {
            PathfindForFoot(i);
            MoveLimb(i);
        }
    }

    // Find the point to move the foot to
    void PathfindForFoot(int index)
    {
        Transform t;

        t = _limbTargets[index];

        if (_limbs[index].sphereObject != null) return; 

        _limbs[index].sphereObject = CreateSphere(index, t);
    }

    void MoveLimb(int index){
        Transform t = _limbTargets[index];
        t.position = _limbs[index].sphereObject.transform.position;
    }


    // Create the target spheres
    GameObject CreateSphere(int index, Transform t)
    {
        // Create the initial game obejct
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Change the name of the game object
        sphere.name = "sphere_" + index.ToString(); // Make the sphere name dynamic

        // Set the size of the target sphere
        sphere.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f); 

        // Get the forward vector of the root object
        Vector3 forwardDirection = _rootObject.transform.forward;

        // Ignore vertical component
        forwardDirection.y = 0f;

        // Normalize the forward vector
        forwardDirection.Normalize();

        // Multiply by desired speed or distance
        Vector3 movement = forwardDirection * _movementSpeed;

        // Set the intial position of the target
        sphere.transform.position = t.position; 
        
        // Move the target in the facing direction
        sphere.transform.position += movement * Time.deltaTime;

        // Return the sphere  
        return sphere;
    }

    // Destroy the target spheres
    void DestroyTargets()
    {
        // Loop through each limb
        for (int i = 0; i < _nLimbs; i++)
        {
            // Destory target
            Destroy(_limbs[i].sphereObject);
            // Remove reference
            _limbs[i].sphereObject = null;
        }
    }

    // Set the legs to rest near the body
    void SetToRest()
    {
    }
}

public class ProceduralLimb
{
    public Transform IKTarget;
    public Vector3 defaultPosition;
    public Vector3 lastPosition;
    public GameObject sphereObject;
    public bool isMoving;
}
