using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum PlayerEnum { Undetermined, Phoenix, River}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public static Dictionary<string, string> DialogueMap = new Dictionary<string, string>()
    {
        { "phoenix-course-1-hole-0-start", "Let the game begin!" },
        { "river-course-1-hole-0-start", "May the best player win!" },
        { "phoenix-course-1-hole-0-complete", "Phoenix for the win!" },
        { "river-course-1-hole-0-complete", "River takes the lead!" },
    };
    
    public Vector2[] course1Scores = new[]
    {
        new Vector2(0, 0), // Hole 1 : x = Player 1's score, y = Player 2's score
        new Vector2(0, 0) // Hole 2
    };

    public Vector2[] course2Scores = new[]
    {
        new Vector2(0, 0),
        new Vector2(0, 0)
    };
    
    public Vector2[] course3Scores = new[]
    {
        new Vector2(0, 0),
        new Vector2(0, 0)
    };
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Instance of GameManager already exists. Destroying this object.");
        }
    }
    
    private void Start()
    {
        
    }

    private void OnEnable()
    {
        // Subscribe to events
        MinigolfGameManager.Instance.OnCourseComplete.AddListener(OnCourseComplete);
    }
    
    private void OnDisable()
    {
        // Unsubscribe from events
    }
    
    #region Event Handlers
    private void OnCourseBegin(MinigolfGameData gameData)
    {
        if (gameData.course == 0 && gameData.hole == 0)
        {
            Debug.Log($"phoenix: {DialogueMap["phoenix-course-1-hole-0-start"]}");
            Debug.Log($"River: {DialogueMap["river-course-1-hole-0-start"]}");
        }
    }

    private void OnHoleBegin(MinigolfGameData gameData)
    {
        
    }
    
    private void OnCourseComplete(MinigolfGameData gameData)
    {
        if (gameData.course == 0 && gameData.hole == 0)
        {
            // DisplayManager.Set(Player.Phoenix, "Let the game begin"!
        }
    }

    private void OnHoleComplete(MinigolfGameData gameData)
    {
        
    }
    
    #endregion
}