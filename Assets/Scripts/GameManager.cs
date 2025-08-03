using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum PlayerEnum { Undetermined, Phoenix, River, LosingPlayer, WinningPlayer, Course2Loser}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
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
    
    private void Start()
    {
        StartCoroutine(StartIntroScene());
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
    
    #region Intro Events

    IEnumerator StartIntroScene()
    {
        yield return DialogueManager.Instance.ShowDialogue("course_0_hole_0_0");
        yield return DialogueManager.Instance.ShowDialogue("course_0_hole_0_1");
        yield return DialogueManager.Instance.ShowDialogue("course_0_hole_0_2");
        yield return DialogueManager.Instance.ShowDialogue("course_0_hole_0_3");
        yield return null;
    }
    #endregion
    
    
    #region Minigolf Game Events
    void OnCourseBegin(MinigolfGameData gameData)
    {
        StartCoroutine(OnCourseBeginCoroutine(gameData));
    }
    
    IEnumerator OnCourseBeginCoroutine(MinigolfGameData gameData)
    {
        // Example of how to trigger parts of the story
        if (gameData.course == 0 && gameData.hole == 0)
        {

        }
        yield return null;
    }

    void OnHoleBegin(MinigolfGameData gameData)
    {
        StartCoroutine(OnHoleBeginCoroutine(gameData));

    }
    
    IEnumerator OnHoleBeginCoroutine(MinigolfGameData gameData)
    {

        yield return null;
    }
    
    void OnCourseComplete(MinigolfGameData gameData)
    {
        StartCoroutine(OnCourseCompleteCoroutine(gameData));
    }
    IEnumerator OnCourseCompleteCoroutine(MinigolfGameData gameData)
    {
        UpdateScore(gameData);

        StartCoroutine(CoOnCourseComplete(gameData));
        
        // Example of how to trigger parts of the story
        if (gameData.course == 0 && gameData.hole == 0)
        {
            // DisplayManager.Set(Player.Phoenix, "Let the game begin"!
        }

        yield return null;
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

    void OnHoleComplete(MinigolfGameData gameData)
    {
        StartCoroutine(OnHoleCompleteCoroutine(gameData));
    }
    IEnumerator OnHoleCompleteCoroutine(MinigolfGameData gameData)
    {
        yield return null;

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