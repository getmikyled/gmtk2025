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
        }
        
        onePlayerButton.onClick.AddListener(OnOnePlayerButtonClicked);
        twoPlayersButton.onClick.AddListener(OnTwoPlayersButtonClicked);
    }

    public void OnOnePlayerButtonClicked()
    {
        if (onePlayerEnabled)
        {
            
        }
        else
        {
            StartCoroutine(DialogueManager.Instance.ShowDialogue("course_0_hole_0_2"));
        }
    }

    public void OnTwoPlayersButtonClicked()
    {
        if (twoPlayerEnabled)
        {
            SceneManager.LoadScene("Game");
            twoPlayerEnabled = false;
            onePlayerEnabled = true;
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(DialogueManager.Instance.ShowDialogue("course_3_hole_0_1"));
        }
    }
}
