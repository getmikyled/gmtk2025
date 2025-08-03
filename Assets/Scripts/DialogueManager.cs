using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    
    [SerializeField] private string dialoguePath1 = "DialogueData/Course1";
    [SerializeField] private string dialoguePath2 = "DialogueData/Course2";
    [SerializeField] private string dialoguePath3 = "DialogueData/Course3";
    
    public List<DialogueData> dialogueData;
    public Dictionary<string, DialogueData> dialogueMap = new Dictionary<string, DialogueData>();

    public PlayerEnum winningPlayer;
    public PlayerEnum losingPlayer;
    
    private bool isDialogueActive = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Instance of ReplayManager already exists. Destroying this  object.");
            return;
        }
        
        Instance = this;

        // Load dialogue in the dictionary so they can be played
        LoadDialogue(dialoguePath1);
        // LoadDialogue(dialoguePath2);
        // LoadDialogue(dialoguePath3);
    }

    public void LoadDialogue(string folderName)
    {
        DialogueData[] loadedData = Resources.LoadAll<DialogueData>(folderName);
        foreach (var item in loadedData)
        {
            dialogueMap.Add(item.id, item);
            dialogueData.Add(item);
        }
    }
    
    public Coroutine ShowDialogue(string id, float duration = 3f)
    {
        return StartCoroutine(ShowDialogueCoroutine(id, duration));
    }

    private IEnumerator ShowDialogueCoroutine(string id, float duration)
    {
        // Wait until previous dialogue finishes
        while (isDialogueActive)
            yield return null;

        if (!dialogueMap.ContainsKey(id))
        {
            Debug.LogWarning($"Dialogue ID '{id}' not found.");
            yield break;
        }

        isDialogueActive = true;

        DialogueData dialogue = dialogueMap[id];
        Debug.Log($"{dialogue.character}: {dialogue.textString}");

        yield return new WaitForSeconds(duration);

        isDialogueActive = false;
    }
}