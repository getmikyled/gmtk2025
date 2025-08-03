using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MinigolfMainMenuUI : MonoBehaviour
{
    private static MinigolfMainMenuUI Instance;

    [SerializeField] private Button onePlayerButton;
    [SerializeField] private Button twoPlayersButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
        else
        {
            Instance = this;
        }
        
        onePlayerButton.onClick.AddListener(OnOnePlayerButtonClicked);
        twoPlayersButton.onClick.AddListener(OnTwoPlayersButtonClicked);
    }

    public void OnOnePlayerButtonClicked()
    {
        Debug.Log("One player button clicked");
        if (MinigolfGameManager.Instance.gameData.course == 0)
        {
            StartCoroutine(DialogueManager.Instance.ShowDialogue("course_0_hole_0_2"));
        }
        else if (MinigolfGameManager.Instance.gameData.course == 1)
        {
            StartCoroutine(DialogueManager.Instance.ShowDialogue("course_2_hole_0_3"));
        }
        else if (MinigolfGameManager.Instance.gameData.course == 2)
        {
            // Beign course 3
            gameObject.SetActive(false);
            StartCoroutine(GameManager.Instance.StartIntroSceneCoroutine());
        }
    }

    public void OnTwoPlayersButtonClicked()
    {
        Debug.Log("Two players button clicked");
        if (MinigolfGameManager.Instance.gameData.course == 0)
        {
            // Begin course 1
            StartCoroutine(DelayStartIntroScene());
        }
        else if (MinigolfGameManager.Instance.gameData.course == 1)
        {
            // Beign course 2
            GameManager.Instance.StartIntroScene();
            gameObject.SetActive(false);
        }
        else if (MinigolfGameManager.Instance.gameData.course == 2)
        {
            StartCoroutine(DialogueManager.Instance.ShowDialogue("course_3_hole_0_1"));
        }
    }
    
    private IEnumerator DelayStartIntroScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");

        // Wait until scene is fully loaded
        while (!asyncLoad.isDone)        {
            Debug.Log("Waiting to load Game scene...");
            yield return null;
        }
        
        // Now the scene is fully loaded, you can start the intro
        Debug.Log("Game scene loaded");
        yield return null; // wait a frame for GameManager to load
        GameManager.Instance.StartIntroScene();
        
        gameObject.SetActive(false);
        yield return null;
    }
}
