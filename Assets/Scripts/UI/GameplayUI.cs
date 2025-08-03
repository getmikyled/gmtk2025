using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public static GameplayUI Instance;
    
    [SerializeField] private Image blackFadeImage;
    [SerializeField] private float lerpFadeDuration = 1.0f;

    [Space] 
    public bool highscoreNameSaved = false;

    [SerializeField] private GameObject highscoreUI;


    [Space]
    [SerializeField] private GameObject golfMainMenuUI;
    
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

        SetGolfMainMenuUIActive(false);
    }

    public void InitializeHighscore()
    {
        highscoreUI.SetActive(true);
        highscoreNameSaved = false;
    }

    public void SetGolfMainMenuUIActive(bool argActive)
    {
        golfMainMenuUI.SetActive(argActive);
    }
    
    /// <summary>
    /// Lerp the fade image from start color to result color.
    /// </summary>
    /// <param name="startColor"></param>
    /// <param name="resultColor"></param>
    /// <returns></returns>
    public IEnumerator CoFadeInAndOut()
    {
        Color black = new Color(0, 0, 0, 1);
        Color clear = new Color(0, 0, 0, 0);
        blackFadeImage.color = clear;

        float timeElapsed = 0f;
        while (timeElapsed < lerpFadeDuration)
        {
            blackFadeImage.color = Color.Lerp(clear, black, timeElapsed / lerpFadeDuration);
            timeElapsed += Time.deltaTime;
            
            yield return null;
        }
        blackFadeImage.color = black;

        yield return new WaitForSeconds(3);
        
        timeElapsed = 0f;
        while (timeElapsed < lerpFadeDuration)
        {
            blackFadeImage.color = Color.Lerp(black, clear, timeElapsed / lerpFadeDuration);
            timeElapsed += Time.deltaTime;
            
            yield return null;
        }
        blackFadeImage.color = clear;
    }

    public void OnSaveHighscoreButtonClicked()
    {
        highscoreUI.SetActive(false);
        highscoreNameSaved = true;
    }
}
