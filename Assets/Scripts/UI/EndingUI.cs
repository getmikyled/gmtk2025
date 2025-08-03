using UnityEngine;
using UnityEngine.UI;

public class EndingUI : MonoBehaviour
{
    [SerializeField] private Button overwriteButton;
    [SerializeField] private Button discardButton;
    
    /// <summary>
    /// </summary>
    void Awake()
    {
        // Subscribe buttons
        overwriteButton.onClick.AddListener(GameplayUI.instance.FadeIn);
        discardButton.onClick.AddListener(GameplayUI.instance.FadeIn);
    }
}
