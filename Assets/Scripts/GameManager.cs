using System.Collections;
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
        MinigolfGameManager.Instance.OnCourseBegin.AddListener(OnCourseBegin);
        MinigolfGameManager.Instance.OnHoleBegin.AddListener(OnHoleBegin);
        MinigolfGameManager.Instance.OnCourseComplete.AddListener(OnCourseComplete);
        MinigolfGameManager.Instance.OnHoleComplete.AddListener(OnHoleComplete);
    }
    
    private void OnDisable()
    {
        // Unsubscribe from events
        MinigolfGameManager.Instance.OnCourseBegin.RemoveListener(OnCourseBegin);
        MinigolfGameManager.Instance.OnHoleBegin.RemoveListener(OnHoleBegin);
        MinigolfGameManager.Instance.OnCourseComplete.RemoveListener(OnCourseComplete);
        MinigolfGameManager.Instance.OnHoleComplete.RemoveListener(OnHoleComplete);
    }
    
    #region Minigolf Game Events
    private void OnCourseBegin(MinigolfGameData gameData)
    {
        // Example of how to trigger parts of the story
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
        UpdateScore(gameData);

        StartCoroutine(CoOnCourseComplete(gameData));
        
        // Example of how to trigger parts of the story
        if (gameData.course == 0 && gameData.hole == 0)
        {
            // DisplayManager.Set(Player.Phoenix, "Let the game begin"!
        }
    }
    
    public IEnumerator CoOnCourseComplete(MinigolfGameData gameData)
    {
        // At the end of courses 1 or 2, prompt user highscore
        if (gameData.course == 0 || gameData.course == 1)
        {
            GameplayUI.Instance.Initialize();

            while (GameplayUI.Instance.highscoreNameSaved == false)
            {
                yield return null;
            }
        }
    }

    private void OnHoleComplete(MinigolfGameData gameData)
    {
        
    }
    
    #endregion
    
    
    #region Highscore Events
    // Index 0 is hole 1, etc. X = Player 1's score, y = Player 2's score
    public Vector2[] course1Scores = new Vector2[2];

    public Vector2[] course2Scores = new Vector2[2];

    public Vector2[] course3Scores = new Vector2[2];

    private void UpdateScore(MinigolfGameData gameData)
    {
        switch (gameData.course)
        {
            case 0:
                course1Scores[0] = new Vector2(gameData.player1Score, gameData.player2Score);
                break;
            case 1:
                course2Scores[0] = new Vector2(gameData.player1Score, gameData.player2Score);
                break;
            case 2:
                course3Scores[0] = new Vector2(gameData.player1Score, gameData.player2Score);
                break;
        }
    }
    
    
    
    #endregion
}