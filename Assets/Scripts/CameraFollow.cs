using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Target for the camera to follow
    public Vector3 offset = new Vector3(0, 25, 0); // The offset from the target

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null) // Make sure the target exists
        {
            transform.position = target.position + offset; // The position of the camera is the position of the target + offset
        }
        
    }
}
