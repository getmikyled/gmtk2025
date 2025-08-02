using System;
using UnityEngine;

public class HoleTrigger : MonoBehaviour
{
    public string ballTag = "GolfBall";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ballTag))
        {
            Debug.Log("Hole completed!");
        }
    }
}
