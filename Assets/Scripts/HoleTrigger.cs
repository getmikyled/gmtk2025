using System;
using UnityEngine;
using UnityEngine.Events;

public class HoleTrigger : MonoBehaviour
{
    public string ballTag = "GolfBall";
    
    public UnityEvent OnHoleCompleted;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ballTag))
        {
            OnHoleCompleted.Invoke();
            Debug.Log("Hole completed!");
        }
    }
    
    
}
