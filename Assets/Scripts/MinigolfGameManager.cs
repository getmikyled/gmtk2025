using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

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

    private GameObject course;  // Instantiated gameobject for minigolf course
    
    public static MinigolfGameManager Instance;
    public MinigolfGameData gameData;
    
    [SerializeField] private BallController player1Ball;
    [SerializeField] private BallController player2Ball;
    [SerializeField] private HoleTrigger hole;
    
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
        StartCoroutine(StartCourseCoroutine());
    }
    
    IEnumerator StartCourseCoroutine()
    {
        Debug.Log($"Starting course: {gameData.course}");

        gameData.hole = 0; // Set to 1st hole
        gameData.currentPlayer = PlayerEnum.Phoenix;
        
        switch (gameData.course)
        {
            case 0:
                course = Instantiate(course1Prefab);
                break;
            case 1:
                Destroy(course);
                course = Instantiate(course2Prefab);
                break;
            case 2:
                Destroy(course);
                course = Instantiate(course2Prefab);
                break;
        }
        
        OnCourseBegin.Invoke(gameData);

        yield return null;
        
        StartHole();
    }

    public void StartHole()
    {
        StartCoroutine(StartHoleCoroutine());
    }
    
    IEnumerator StartHoleCoroutine()
    {
        switch (gameData.course)
        {
            case 0:
                break;
            case 1:
                break;
        }
        
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

    IEnumerator EndHole()
    {
        OnHoleComplete.Invoke(gameData);

        yield return null; // Wait 1 frame to resolve OnHoleComplete events
        
        if (gameData.hole >= 2) // At last hole
        {
            EndCourse();
            yield break;
        }

        if (gameData.currentPlayer == PlayerEnum.Phoenix)
        {
            // When 1st player finishes, repeat hole with 2nd player
            gameData.currentPlayer = PlayerEnum.River;
        }
        else
        {
            // Otherwise, advance to next hole and begin player 1
            gameData.currentPlayer = PlayerEnum.Phoenix;
            gameData.hole++;
        }
        
        OnChangeTurn.Invoke(gameData);
        yield return null;
    }
    
    
    // Update data helper functions
    private void UpdateWinnerLoserData()
    {
        gameData.winningPlayer = (gameData.player1Score > gameData.player2Score) ? PlayerEnum.Phoenix : PlayerEnum.River;
        gameData.losingPlayer = (gameData.player1Score < gameData.player2Score) ? PlayerEnum.Phoenix : PlayerEnum.River;
    }

    #region Minigolf Game Events
    public void OnBallEnteredHole(/* BallController ballRef,  */)
    {
        EndHole();
    }

    public void OnBallHit()
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
