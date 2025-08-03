using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingUI : MonoBehaviour
{
    [SerializeField] private Button overwriteButton;
    [SerializeField] private Button discardButton;
    
    /// <summary>
    /// </summary>
    void Awake()
    {
        discardButton.onClick.AddListener(() =>
        {
            StartCoroutine(OnDiscardButtonClicked());
        });
        
        overwriteButton.onClick.AddListener(() =>
        {
            StartCoroutine(OnOverwriteButtonClicked());
        });
    }

    IEnumerator OnDiscardButtonClicked()
    {
        yield return DialogueManager.Instance.ShowDialogue("course_3_hole_2_22");
        yield return DialogueManager.Instance.ShowDialogue("course_3_hole_2_23");
        yield return GameplayUI.Instance.CoFadeInAndOut();
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator OnOverwriteButtonClicked()
    {
        yield return DialogueManager.Instance.ShowDialogue("course_3_hole_2_24");
        yield return DialogueManager.Instance.ShowDialogue("course_3_hole_2_25");
        yield return GameplayUI.Instance.CoFadeInAndOut();
        SceneManager.LoadScene("MainMenu");
    }
}
