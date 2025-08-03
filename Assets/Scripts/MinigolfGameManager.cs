using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Object = System.Object;

[Serializable]
public class MinigolfGameData
{
    public PlayerEnum currentPlayer;
    public int course;
    public int hole;
    public PlayerEnum winningPlayer;
    public PlayerEnum losingPlayer;
    public int player1Score = 0;
    public int player2Score = 0;
}

public class MinigolfGameManager : MonoBehaviour
{
    public GameObject course1Prefab;  // Reference to the prefab of mingolf course
    public GameObject course2Prefab;

    private CourseManager courseManager;
    private Course course;  // Instantiated gameobject for minigolf course
    
    public static MinigolfGameManager Instance;
    public MinigolfGameData gameData;
    
    // Events
    public UnityEvent<MinigolfGameData> OnCourseBegin;
    public UnityEvent<MinigolfGameData> OnHoleBegin;
    public UnityEvent<MinigolfGameData> OnCourseComplete;
    public UnityEvent<MinigolfGameData> OnHoleComplete;
    public UnityEvent<MinigolfGameData> OnChangeTurn;
    
    
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
        // Initialize starting properties
        gameData = new MinigolfGameData();
        gameData.currentPlayer = PlayerEnum.Phoenix;
        gameData.course = 0;
        gameData.hole = 0;
        gameData.winningPlayer = PlayerEnum.Undetermined;
        gameData.losingPlayer = PlayerEnum.Undetermined;
        gameData.player1Score = 0;
        gameData.player2Score = 0;
    }

    public void StartCourse()
    {
        if (courseManager == null)
        {
            courseManager = GameObject.FindGameObjectWithTag("Courses").GetComponent<CourseManager>();
        }

        StartCoroutine(StartCourseCoroutine());
    }
    
    IEnumerator StartCourseCoroutine()
    {
        Debug.Log($"Starting course: {gameData.course}");

        gameData.hole = 0; // Set to 1st hole
        gameData.currentPlayer = PlayerEnum.Phoenix;
        
        UnloadCourse();
        
        switch (gameData.course)
        {
            case 0:
                course = courseManager.course1;
                break;
            case 1:
                
                course = courseManager.course2;
                break;
            case 2:
                course = courseManager.course2;
                break;
        }
        if (course == null)
        {
            Debug.LogError("Course1 not found.");
        }
        
        course.GameObject().SetActive(true);
        course.ActivateHole(0); // Activate the first hole
        
        // Subscribe to the balls' events
        course.currentHole.ball.OnBallMoved.AddListener(HandleOnBallMoved);
        course.currentHole.hole.OnHoleCompleted.AddListener(HandleOnHoleCompleted);
        
        OnCourseBegin.Invoke(gameData);

        yield return null;
        
        StartHole();
    }

    public void UnloadCourse()
    {
        if (course == null) return; // If there is no course to unload, return (no need to unload)
        
        course.currentHole.ball.OnBallMoved.RemoveListener(HandleOnBallMoved);
        course.currentHole.hole.OnHoleCompleted.RemoveListener(HandleOnHoleCompleted);
        
        course.gameObject.SetActive(false);
    }

    public void StartHole()
    {
        StartCoroutine(StartHoleCoroutine());
    }
    
    IEnumerator StartHoleCoroutine()
    {
        switch (gameData.currentPlayer)
        {
            case PlayerEnum.Phoenix:
                break;
            case PlayerEnum.River:
                break; 
        }
        course.ActivateHole(gameData.hole);
        
        OnHoleBegin.Invoke(gameData);
        yield return null;
    }

    public void EndCourse()
    {
        StartCoroutine(EndCourseCoroutine());
    }

    IEnumerator EndCourseCoroutine()
    {
        Destroy(course);
        OnCourseComplete.Invoke(gameData);

        yield return null; // Wait 1 frame to resolve OnCourseComplete events
        
        // Move to next
        gameData.course++;
    }

    public void EndHole()
    {
        StartCoroutine(EndHoleCoroutine());
    }

    IEnumerator EndHoleCoroutine()
    {
        OnHoleComplete.Invoke(gameData);
        Debug.Log("OnHoleComplete event invoked.");

        yield return null; // Wait 1 frame to resolve OnHoleComplete events
        
        if (gameData.hole >= 2) // At last hole
        {
            Debug.Log("Last hole reached, ending course.");
            EndCourse();
            yield break;
        }

        if (gameData.currentPlayer == PlayerEnum.Phoenix)
        {
            // When 1st player finishes, repeat hole with 2nd player
            Debug.Log("Current player is Phoenix, switching to River.");
            gameData.currentPlayer = PlayerEnum.River;
        }
        else
        {
            // Otherwise, advance to next hole and begin player 1
            Debug.Log("Current player is River, advancing to the next hole and switching to Phoenix.");
            gameData.currentPlayer = PlayerEnum.Phoenix;
            gameData.hole++;
        }
        
        OnChangeTurn.Invoke(gameData);
        Debug.Log("OnChangeTurn event invoked.");
        
        // Start hole again for next player
        StartHole();
        Debug.Log("StartHole method called for the next player.");
        
        yield return null;
    }

    public void ResetHole()
    {
        // Place ball at restart
        var ball = course.currentHole.ball;
        ball.transform.position = ball.startingPosition;
    }
    
    
    // Update data helper functions
    private void UpdateWinnerLoserData()
    {
        gameData.winningPlayer = (gameData.player1Score > gameData.player2Score) ? PlayerEnum.Phoenix : PlayerEnum.River;
        gameData.losingPlayer = (gameData.player1Score < gameData.player2Score) ? PlayerEnum.Phoenix : PlayerEnum.River;
    }

    #region Minigolf Game Events
    public void HandleOnHoleCompleted(/* BallController ballRef,  */)
    {
        EndHole();
    }

    public void HandleOnBallMoved(BallController ballRef)
    {
        if (gameData.currentPlayer == PlayerEnum.Phoenix)
        {
            gameData.player1Score++;
        }
        else
        {
            gameData.player2Score++;
        } 
    }
    
    #endregion
}
