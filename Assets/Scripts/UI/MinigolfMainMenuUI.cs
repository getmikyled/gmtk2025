using UnityEngine;

public class MinigolfMainMenuUI : MonoBehaviour
{
    private static MinigolfMainMenuUI Instance;
    
    public bool onePlayerEnabled = false;
    public bool twoPlayerEnabled = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Persist across scene loads
        }
    }
}
