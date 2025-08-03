using UnityEngine;

public class Course : MonoBehaviour
{
    
    public HoleLevel holeLevel1;
    public HoleLevel holeLevel2;
    
    [Header("Runtime")]
    public HoleLevel currentHole;

    private void Start()
    {
        ActivateHole(0);
    }
        
    public void ActivateHole(int holeIndex)
    {
        
        // Deactivate currentHole, if existing
        currentHole?.gameObject.SetActive(false);
        
        switch (holeIndex)
        {
            case 0:
                currentHole = holeLevel1;
                break;
            case 1:
                currentHole = holeLevel2;
                break;
            default:
                Debug.LogError($"Attempting to load nonexistent hole: {holeIndex}. Must be fixed; this should not be called.");
                break;
        }
        
        // Activate current hole
        currentHole?.gameObject.SetActive(true);
    }
}
