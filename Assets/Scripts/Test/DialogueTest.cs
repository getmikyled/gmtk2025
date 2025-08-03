using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class DialogueTest : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(PlayDialogueSequence());
        
    }

    public IEnumerator PlayDialogueSequence()
    {
        yield return DialogueManager.Instance.ShowDialogue("course_0_hole_0_0");
        yield return DialogueManager.Instance.ShowDialogue("course_0_hole_0_1");
    }
}
