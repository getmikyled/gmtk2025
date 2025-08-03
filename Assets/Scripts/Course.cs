using UnityEngine;

public class Course : MonoBehaviour
{

    public HoleLevel currentHole;
    public HoleLevel holeLevel1;
    public HoleLevel holeLevel2;
    
        
    public void ActivateHole(int holeIndex)
    {
        switch (holeIndex)
        {
            case 1:
                currentHole = holeLevel1;
                break;
            case 2:
                currentHole = holeLevel2;
                break;
            default:
                Debug.LogError($"Attempting to load nonexistent hole: {holeIndex}. Must be fixed; this should not be called.");
                break;
        }
        
        
        // Deactivate the other hole game object. Activate this one. 
        
        currentHole.gameObject.SetActive(true);
    }
}
