using System;
using UnityEngine;
using UnityEngine.Events;

// Every Unity script that interacts with the engine must inherit from MonoBehaviour
public class BallController : MonoBehaviour
{
    private Rigidbody rb; // Rigid body of the sphere
    private Vector3 dragStartPos; // Will store mouse position at beginning of the drag
    private Vector3 dragEndPos; // Will store mouse position at the end of the drag
    private bool isDragging = false; // Flag for dragging
    private bool isClickedOnBall = false; // Flag for if the click starts on ball
    private bool isMoving = false; // Flag for if the ball is moving
    public float power = 10f; // Multiplier for power

    public UnityEvent<BallController> OnBallMove; // Event for when the ball moves
    
    void Start() // Called once at the start of the game (required for all MonoBehavior scripts)
    {
        rb = GetComponent<Rigidbody>(); // Attaches the Rigidbody variable to the sphere
    }

    void Update() // Called during every frame 
    {
        if (Input.GetMouseButtonDown(0) && !isMoving) // Check if the mouse was clicked down
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Create ray from camera to mouse position
            if (Physics.Raycast(ray, out RaycastHit hit)) // Set variable hit to be position of where the ray hits something
            {
                if (hit.collider.gameObject == gameObject) // Check if the hit is on a game object
                {
                    // Set is dragging to true and get the mouse position
                    Debug.Log("Clicked on ball");
                    isDragging = true;
                    isClickedOnBall = true;
                    dragStartPos = GetMouseWorldPosition();
                }
                else
                {
                    Debug.Log("Collision elsewhere");
                }
            }
            else
            {
                Debug.Log("No collision");
            }
            
        }

        if (Input.GetMouseButtonUp(0) && isDragging && isClickedOnBall && !isMoving) // Check if the mouse was released while being dragged
        {
            // Set dragging to false, get mouse position, create a force and apply it to the ball
            // The force accounts for direction already
            dragEndPos = GetMouseWorldPosition();
            Vector3 force = (dragStartPos - dragEndPos) * power;
            
            rb.AddForce(force, ForceMode.Impulse);
            
            // Invoke ball move event so listeners have access to the data
            OnBallMove?.Invoke(this);
            Debug.Log($"[BallController ({Time.frameCount})] Moving ball");

            isDragging = false;
            isClickedOnBall = false;
        }
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    Vector3 GetMouseWorldPosition() // Converts 2D mouse position to 3D world position
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Creates a ray from the camera through the mouse's position on screen
        Plane plane = new Plane(Vector3.up, Vector3.zero); // Creates a horizontal plane at y = 0 in world space
        if (plane.Raycast(ray, out float distance)) // Checks if the ray intersects the plane; if so, 'distance' is the length from ray origin to intersection
        {
            return ray.GetPoint(distance); // Returns the 3D world position where the ray hits the plane
        }
        return Vector3.zero; // Default return value if ray doesn't hit (shouldn't happen unless camera is oddly placed)
    }
}
