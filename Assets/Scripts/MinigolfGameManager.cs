using System;
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
    public static MinigolfGameManager Instance;
    public MinigolfGameData gameData;

    [SerializeField] private BallController player1Ball;
    [SerializeField] private BallController player2Ball;
    
    // Events
    public UnityEvent<MinigolfGameData> OnCourseBegin;
    public UnityEvent<MinigolfGameData> OnGameCBegin;
    public UnityEvent<MinigolfGameData> OnCourseComplete;
    public UnityEvent<MinigolfGameData> OnHoleComplete;
    public UnityEvent<MinigolfGameData> OnChangeTurn;
    
    
    public void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Instance of MinigolfGameManager already exists. Destroying this object.");
        }
    }
    
    public void Start()
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
        Debug.Log($"Starting course: {gameData.course}");

        gameData.hole = 0; // Set to 1st hole
        
        switch (gameData.course)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }

    public void StartHole()
    {
        switch (gameData.course)
        {
            case 0:
                break;
            case 1:
                break;
        }
    }

    private void LoadCourse(int course)
    {
        
    }

    private void LoadHole(int hole)
    {
        
    }
    
    
    // Update data helper functions
    private void UpdateWinnerLoserData()
    {
        gameData.winningPlayer = (gameData.player1Score > gameData.player2Score) ? PlayerEnum.Phoenix : PlayerEnum.River;
        gameData.losingPlayer = (gameData.player1Score < gameData.player2Score) ? PlayerEnum.Phoenix : PlayerEnum.River;
    }

    public void OnBallEnteredHole(/* BallController ballRef,  */)
    {
        OnHoleComplete.Invoke(gameData);
        
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
    
    
}
