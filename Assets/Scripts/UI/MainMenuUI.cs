using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    /// <summary>
    /// Called when the play button is clicked.
    /// </summary>
    private void OnPlayButtonClicked()
    {
        Debug.Log("Play");
    }

    /// <summary>
    /// Called when the exit button is clicked.
    /// </summary>
    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
