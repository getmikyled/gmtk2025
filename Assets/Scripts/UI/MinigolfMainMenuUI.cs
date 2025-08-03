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
            StartCoroutine(GameManager.Instance.StartIntroScene());
        }
    }

    public void OnTwoPlayersButtonClicked()
    {
        if (MinigolfGameManager.Instance.gameData.course == 0)
        {
            // Begin course 1
            SceneManager.LoadScene("Game");
            StartCoroutine(GameManager.Instance.StartIntroScene());
            gameObject.SetActive(false);
        }
        else if (MinigolfGameManager.Instance.gameData.course == 1)
        {
            // Beign course 2
            gameObject.SetActive(false);
            StartCoroutine(GameManager.Instance.StartIntroScene());
        }
        else if (MinigolfGameManager.Instance.gameData.course == 2)
        {
            StartCoroutine(DialogueManager.Instance.ShowDialogue("course_3_hole_0_1"));
        }
    }
}
