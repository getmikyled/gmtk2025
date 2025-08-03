using System.Collections.Generic;
using UnityEngine;

public enum SequenceType {YesStart, YesEnd, Yes, No}

public class DialogueData : ScriptableObject
{
    public string id;
    public PlayerEnum character;
    public string textString;
    public SequenceType sequenceType;
}

