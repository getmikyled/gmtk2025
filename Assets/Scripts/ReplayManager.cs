using System;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ReplayManager : MonoBehaviour
{
    public static ReplayManager Instance;
    
    public enum ReplayState {Inactive, PlayStarted, PlayEnded }
    public ReplayState state;
    
    [Serializable] 
    public class RecordingData : ScriptableObject
    {
        public BallController ballRef;
        public List<MovementData> movements;
    }
    
    public Dictionary<string, RecordingData> recordingData;
    public string replayName;
    public int replayIndex;

    public UnityEvent<MovementData> onBeginReplay;
    public UnityEvent<MovementData> onPlayNextMovement;
    public UnityEvent onEndReplay;
    
    
    public void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Instance of ReplayManager already exists. Destroying this  object.");
            return;
        }
        
        Instance = this;
        recordingData = new Dictionary<string, RecordingData>();  
    } 
    

    // TODO maybe wrap the ball and the list of positions into a wrapper class. It should be a ball and a list of 
    // positions for each movement. Maybe use ID ball-course-hole-move
    List<Vector3> tempPositions = new List<Vector3>();
    private BallController tempBall;
    private bool isRecording = false;
    public async Task RecordMovement(string id, BallController ball)
    {
        if (recordingData.ContainsKey(id) == false)
        {
            // Create a completely new recording
            recordingData.Add(id, ScriptableObject.CreateInstance<RecordingData>());
            recordingData[id].movements = new List<MovementData>();
        }
        
        // Recording movement for existing hole
        var currentMovement = ScriptableObject.CreateInstance<MovementData>();
        currentMovement.positions = new List<Vector3>();

        recordingData[id].ballRef = ball;
        
        isRecording = true;
        Debug.Log($"[ReplayManager ({Time.frameCount})] Start recording");
        
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        float stopThreshold = 0.001f;
        
        // Wait until the Rigidbody actually starts moving
        while (rb.linearVelocity.magnitude <= stopThreshold)
        {
            await Task.Yield();
        }
        
        // Record position as it moves
        while (isRecording)
        {
            Debug.Log($"[ReplayManager ({Time.frameCount})] Recording: Position: {ball.transform.position}");
            // tempPositions.Add(ball.transform.position); 
            currentMovement.positions.Add(ball.transform.position);
            if (rb.linearVelocity.magnitude > stopThreshold)
            {
                
                await Task.Yield();
            }
            else
            {
                isRecording = false;
            }
        }
        
        // Store the recorded movements into the dictionary
        recordingData[id].movements.Add(currentMovement);
        Debug.Log($"[ReplayManager ({Time.frameCount})] End recording.");
        
    }
    
    #region Replay Sequence

    public async Task BeginReplay(string id)
    {

        if (recordingData.ContainsKey(id) == false || recordingData[id].movements.Count == 0)
        {
            Debug.Log($"Recording '{id}' does not exist or no movements stored. Canceling replay.");
            CancelReplay();
            return;
        }
        
        replayName = id;
        replayIndex = 0;
        state = ReplayState.PlayStarted;
        
        var currentMovement = recordingData[id].movements[replayIndex];
        onBeginReplay.Invoke(currentMovement);
        
        await MoveBall(recordingData[id].ballRef.transform, currentMovement.positions);
        replayIndex++;
    }

    public async Task MoveBall(Transform ballRef, List<Vector3> positions)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            ballRef.transform.position = positions[i];
            await Task.Yield();
        }
    }

    public async Task PlayNextMovement()
    {
        if (replayIndex >= recordingData[replayName].movements.Count)
        {
            Debug.Log($"Recording '{replayName}' has no more moves left. Ending replay.");
            EndReplay(); // At the end, invoke ending sequence
            return;
        }
        
        Debug.Log($"Playing movement {replayIndex}: {recordingData}");
        await MoveBall(recordingData[replayName].ballRef.transform, recordingData[replayName].movements[replayIndex].positions);
        // onPlayNextMovement.Invoke(movementData[replayName][replayIndex]);
        replayIndex++;
    }

    public void EndReplay()
    {
        state = ReplayState.PlayEnded;
        onEndReplay.Invoke();
    }

    public void CancelReplay()
    {
        Debug.Log("Cancel Replay");
        state = ReplayState.Inactive;
    }

    public void Reset()
    {
        state = ReplayState.Inactive;
    }

    #endregion
}
