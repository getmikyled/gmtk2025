using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class ReplayManager : MonoBehaviour
    {
        public static ReplayManager Instance;
        
        public enum ReplayState {Inactive, PlayStarted, PlayEnded }
        public ReplayState state;
        
        public Dictionary<string, List<MovementData>> movementData;
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
            movementData = new Dictionary<string, List<MovementData>>();  
        } 
        
        /// <summary>
        /// Likely called by MObrmrny 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public void RecordMovement(string id, MovementData data)
        {
            if (movementData.ContainsKey(id) == false)
            {
                movementData.Add(id, new List<MovementData>());
            }
            
            movementData[id].Add(data);
        }

        
        #region Replay Sequence
        public void BeginReplay(string id)
        {

            if (movementData.ContainsKey(id) == false || movementData[id].Count == 0)
            {
                CancelReplay();
            }
            
            replayName = id;
            replayIndex = 0;
            state = ReplayState.PlayStarted;
            
            onBeginReplay.Invoke(movementData[id][replayIndex]);
        }

        public void PlayNextMovement()
        {
            if (replayIndex >= movementData[replayName].Count)
            {
                EndReplay(); // At the end, invoke ending sequence
                return;
            }
            
            Debug.Log($"Playing movement {replayIndex}: {movementData}");
            onPlayNextMovement.Invoke(movementData[replayName][replayIndex]);
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
}