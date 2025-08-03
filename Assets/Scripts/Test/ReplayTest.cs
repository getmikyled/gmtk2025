using System.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace
{
    public class ReplayTest : MonoBehaviour
    {
        public BallController ball1;
        
        private ReplayManager replayManager;
        
        private void Start()
        {
            replayManager = ReplayManager.Instance;
            
            // Add Listeners
            replayManager.onBeginReplay.AddListener(HandleBeginReplay);
            replayManager.onPlayNextMovement.AddListener(HandlePlayNextMovement);
            replayManager.onEndReplay.AddListener(HandleEndReplay);
            
            // ball1.OnBallMove += HandleOnBallMove;
            ball1.OnBallMoved.AddListener(HandleOnBallMoveBegin);
        }

        private int courseIndex = 0;
        private int holeIndex = 0;
        private string recordingId;
        public async void HandleOnBallMoveBegin(BallController ball)
        {
            await Task.Yield();
            recordingId = $"{ball.name}-course-{courseIndex}-hole-{holeIndex}";
            await replayManager.RecordMovement(recordingId, ball);
        }

        private void Update()
        {
            // TEST: REPLAY 
            if (Input.GetKeyUp(KeyCode.R))
            {
                switch (replayManager.state)
                {
                    case ReplayManager.ReplayState.Inactive:
                        replayManager.BeginReplay(recordingId);
                        break;
                    case ReplayManager.ReplayState.PlayStarted:
                        replayManager.PlayNextMovement();
                        break;
                    case ReplayManager.ReplayState.PlayEnded:
                        replayManager.EndReplay();
                        break;
                }
            }
        }
        
        
        

        private void HandleBeginReplay(BallMovementData ballMovementData)
        {
            ball1.GetComponent<Renderer>().material.color = Color.green; // TODO temporary for debugging
        }
        
        private void HandlePlayNextMovement(BallMovementData ballMovementData)
        {
            Debug.Log("Play Next Movement!");
        }
        
        private void HandleEndReplay()
        {
            Debug.Log("End Replay!");
        }
    }
}