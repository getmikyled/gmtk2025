using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject uiContainer;
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;

    [Space] [SerializeField] private float cameraIntroSequenceDuration = 2f;
    private Vector3 cameraStartPosition = new Vector3(-9.48f, 8.94f, -22.58f);
    private Vector3 cameraEndPosition = new Vector3(-0.8f, 9.05f, 3.12f);
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
        Camera.main.transform.position = cameraStartPosition;
    }

    /// <summary>
    /// Called when the play button is clicked.
    /// </summary>
    private void OnPlayButtonClicked()
    {
        uiContainer.SetActive(false);
        StartCoroutine(CoLerpIntroCameraSequence());
    }

    /// <summary>
    /// Called when the exit button is clicked.
    /// </summary>
    private void OnExitButtonClicked()
    {
        Application.Quit();
    }

    private IEnumerator CoLerpIntroCameraSequence()
    {
        Camera mainCamera = Camera.main;
        mainCamera.transform.localPosition = cameraStartPosition;
        
        float timeElapsed = 0f;
        while (timeElapsed < cameraIntroSequenceDuration)
        {
            Camera.main.transform.localPosition = Vector3.Lerp(cameraStartPosition, cameraEndPosition, timeElapsed / cameraIntroSequenceDuration);

            timeElapsed += Time.deltaTime;

            yield return null;
        }
        
        mainCamera.transform.localPosition = cameraEndPosition;
        GameplayUI.Instance.SetGolfMainMenuUIActive(true);
    }
}
