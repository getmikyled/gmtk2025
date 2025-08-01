using System;
using System.Net;
using UnityEngine;

[Serializable]
public class MovementData : ScriptableObject
{
    public Vector3 velocity;
    public Vector3 startPoint;
    public Vector3 endPoint;
}