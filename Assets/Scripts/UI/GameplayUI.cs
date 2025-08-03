using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private Image blackFadeImage;
    [SerializeField] private float lerpFadeDuration = 1.0f;

    /// <summary>
    /// Fade the screen to black
    /// </summary>
    public void FadeIn()
    {
        Color startColor = new Color(0, 0, 0, 1);
        Color resultColor = new Color(0, 0, 0, 0);
        StartCoroutine(CoLerpImageColor(startColor, resultColor));
    }
    
    /// <summary>
    /// Fade out the black screen
    /// </summary>
    public void FadeOut()
    {
        Color startColor = new Color(0, 0, 0, 0);
        Color resultColor = new Color(0, 0, 0, 1);
        StartCoroutine(CoLerpImageColor(startColor, resultColor));
    }
    
    /// <summary>
    /// Lerp the fade image from start color to result color.
    /// </summary>
    /// <param name="startColor"></param>
    /// <param name="resultColor"></param>
    /// <returns></returns>
    private IEnumerator CoLerpImageColor(Color startColor, Color resultColor)
    {
        blackFadeImage.color = startColor;

        float timeElapsed = 0f;
        while (timeElapsed < lerpFadeDuration)
        {
            blackFadeImage.color = Color.Lerp(startColor, resultColor, timeElapsed / lerpFadeDuration);
            timeElapsed += Time.deltaTime;
            
            yield return null;
        }
        blackFadeImage.color = resultColor;
    }
}
