using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        // Subscribe to events
        MinigolfGameManager.Instance.OnCourseBegin.AddListener(OnCourseBegin);
        MinigolfGameManager.Instance.OnHoleBegin.AddListener(OnHoleBegin);
        MinigolfGameManager.Instance.OnCourseComplete.AddListener(OnCourseComplete);
        MinigolfGameManager.Instance.OnHoleComplete.AddListener(OnHoleComplete);
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        MinigolfGameManager.Instance.OnCourseBegin.RemoveListener(OnCourseBegin);
        MinigolfGameManager.Instance.OnHoleBegin.RemoveListener(OnHoleBegin);
        MinigolfGameManager.Instance.OnCourseComplete.RemoveListener(OnCourseComplete);
        MinigolfGameManager.Instance.OnHoleComplete.RemoveListener(OnHoleComplete);
    }
    
    #region Intro Events

    public IEnumerator StartIntroScene()
    {
        MinigolfGameManager.Instance.StartCourse();
        
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
        Debug.Log($"[GameManager ({Time.frameCount})] OnCourseBeginCoroutine - Course: {gameData.course}");
        if (gameData.course == 0 && gameData.hole == 0)
        {
            yield return DialogueManager.Instance.ShowDialogue("course_0_hole_0_3");
            yield return new WaitForSeconds(4);
            yield return DialogueManager.Instance.ShowDialogue("course_1_hole_1_4");
            yield return DialogueManager.Instance.ShowDialogue("course_1_hole_1_5");
            yield return new WaitForSeconds(4);
            yield return DialogueManager.Instance.ShowDialogue("course_1_hole_1_6");
            yield return DialogueManager.Instance.ShowDialogue("course_1_hole_1_7");
            yield return DialogueManager.Instance.ShowDialogue("course_1_hole_1_8");
            yield return DialogueManager.Instance.ShowDialogue("course_1_hole_1_9");
            yield return DialogueManager.Instance.ShowDialogue("course_1_hole_1_10");
            yield return DialogueManager.Instance.ShowDialogue("course_1_hole_1_11");
            yield return DialogueManager.Instance.ShowDialogue("course_1_hole_1_12");
        }
        else if (gameData.course == 1 && gameData.hole == 0)
        {
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_1_4");
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_1_5");
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_1_6");
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_1_7");
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_1_8");
        }
        else if (gameData.course == 2 && gameData.hole == 0)
        {
            yield return DialogueManager.Instance.ShowDialogue("course_3_hole_0_3");
            yield return DialogueManager.Instance.ShowDialogue("course_3_hole_1_4");
            yield return DialogueManager.Instance.ShowDialogue("course_3_hole_1_5");
            yield return DialogueManager.Instance.ShowDialogue("course_3_hole_1_6");
        }
        yield return null;
    }

    void OnHoleBegin(MinigolfGameData gameData)
    {
        StartCoroutine(OnHoleBeginCoroutine(gameData));
    }
    
    IEnumerator OnHoleBeginCoroutine(MinigolfGameData gameData)
    {
        Debug.Log($"[GameManager ({Time.frameCount})] OnHoleBeginCoroutine - Hole: {gameData.hole}");
        yield return null;
    }
    
    void OnCourseComplete(MinigolfGameData gameData)
    {
        StartCoroutine(OnCourseCompleteCoroutine(gameData));
    }
    IEnumerator OnCourseCompleteCoroutine(MinigolfGameData gameData)
    {
        Debug.Log($"[GameManager ({Time.frameCount})] OnCourseCompleteCoroutine - Course: {gameData.course}");
        UpdateScore(gameData);

        if (gameData.course == 0)
        {
            switch (gameData.winningPlayer)
            {
                case PlayerEnum.Phoenix:
                    yield return DialogueManager.Instance.ShowDialogue("course_1_hole_2_18");
                    yield return DialogueManager.Instance.ShowDialogue("course_1_hole_2_19");
                    break;
                case PlayerEnum.River:
                    yield return DialogueManager.Instance.ShowDialogue("course_1_hole_2_21");
                    yield return DialogueManager.Instance.ShowDialogue("course_1_hole_2_22");
                    yield return DialogueManager.Instance.ShowDialogue("course_1_hole_2_23");
                    yield return DialogueManager.Instance.ShowDialogue("course_1_hole_2_24");
                    break;
            }
            
            GameplayUI.Instance.InitializeHighscore();
            while (GameplayUI.Instance.highscoreNameSaved == false)
            {
                yield return null;
            }
            yield return GameplayUI.Instance.CoFadeInAndOut();
            GameplayUI.Instance.SetGolfMainMenuUIActive(true);
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_0_0");
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_0_1");
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_0_2");
        }
        else if (gameData.course == 1)
        {
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_2_16");
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_2_17");
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_2_18");
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_2_19");
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_2_20");
            
            GameplayUI.Instance.InitializeHighscore();
            while (GameplayUI.Instance.highscoreNameSaved == false)
            {
                yield return null;
            }
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_2_21");
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_2_22");
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_2_23");
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_2_24");
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_2_25");
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_2_26");
            yield return GameplayUI.Instance.CoFadeInAndOut();
            GameplayUI.Instance.SetGolfMainMenuUIActive(true);
            yield return DialogueManager.Instance.ShowDialogue("course_3_hole_0_0");
        }
        else if (gameData.course == 2)
        {
            yield return DialogueManager.Instance.ShowDialogue("course_3_hole_2_15");
            yield return DialogueManager.Instance.ShowDialogue("course_3_hole_2_16");
            yield return DialogueManager.Instance.ShowDialogue("course_3_hole_2_17");
            yield return DialogueManager.Instance.ShowDialogue("course_3_hole_2_19");
            yield return DialogueManager.Instance.ShowDialogue("course_3_hole_2_20");
            yield return DialogueManager.Instance.ShowDialogue("course_3_hole_2_21");
            yield return GameplayUI.Instance.CoFadeInAndOut();
            SceneManager.LoadScene("Ending");
        }

        yield return null;
    }

    void OnHoleComplete(MinigolfGameData gameData)
    {
        StartCoroutine(OnHoleCompleteCoroutine(gameData));
    }
    IEnumerator OnHoleCompleteCoroutine(MinigolfGameData gameData)
    {
        Debug.Log($"[GameManager ({Time.frameCount})] OnHoleCompleteCoroutine - Hole: {gameData.hole}");
        yield return null;

        if (gameData.course == 0 && gameData.hole == 0)
        {
            yield return DialogueManager.Instance.ShowDialogue("course_1_hole_1_13");
            yield return DialogueManager.Instance.ShowDialogue("course_1_hole_1_14");
        }
        else if (gameData.course == 0 && gameData.hole == 1)
        {
            yield return DialogueManager.Instance.ShowDialogue("course_1_hole_2_15");
            yield return DialogueManager.Instance.ShowDialogue("course_1_hole_2_16");
            yield return DialogueManager.Instance.ShowDialogue("course_1_hole_2_17");
        }
        else if (gameData.course == 1 && gameData.hole == 0)
        {
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_1_9");
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_1_10");
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_1_11");
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_1_12");
        }
        else if (gameData.course == 1 && gameData.hole == 1)
        {
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_1_13");
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_2_14");
            yield return DialogueManager.Instance.ShowDialogue("course_2_hole_2_15");
        }
        else if (gameData.course == 2 && gameData.hole == 0)
        {
            Debug.Log("We don't have a way to determine if ghost wins or not on these next 3 lines");
            yield return DialogueManager.Instance.ShowDialogue("course_3_hole_1_7");
            yield return DialogueManager.Instance.ShowDialogue("course_3_hole_1_8");
            yield return DialogueManager.Instance.ShowDialogue("course_3_hole_1_9");
        }
        else if (gameData.course == 2 && gameData.hole == 1)
        {
            Debug.Log("We don't have a way to determine if ghost wins or not on these next lines");
            switch (gameData.winningPlayer)
            {
                case PlayerEnum.Phoenix:
                    yield return DialogueManager.Instance.ShowDialogue("course_3_hole_2_10");
                    break;
                case PlayerEnum.River:
                    yield return DialogueManager.Instance.ShowDialogue("course_3_hole_2_11");
                    yield return DialogueManager.Instance.ShowDialogue("course_3_hole_2_12");
                    break;
            }
            yield return DialogueManager.Instance.ShowDialogue("course_3_hole_2_13");
            yield return DialogueManager.Instance.ShowDialogue("course_3_hole_2_14");
        }
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