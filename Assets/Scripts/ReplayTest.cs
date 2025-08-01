using UnityEngine;

namespace DefaultNamespace
{
    public class ReplayTest : MonoBehaviour
    {
        public GameObject ball1;
        
        private ReplayManager replayManager;
        
        private void Start()
        {
            replayManager = ReplayManager.Instance;
            
            // Add Listeners
            replayManager.onBeginReplay.AddListener(HandleBeginReplay);
            replayManager.onPlayNextMovement.AddListener(HandlePlayNextMovement);
            replayManager.onEndReplay.AddListener(HandleEndReplay);
        }

        private void Update()
        {
            // TEST: BALL
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Debug.Log("Space");

                int x = Random.Range(3, 10);
                int z = Random.Range(3, 10);
                Vector3 newPosition = new Vector3(x, 0, z);

                MovementData movementData = new MovementData();
                movementData.startPoint = ball1.transform.position;
                movementData.velocity = new Vector3(0, 0, 0); // TODO Implement velocity
                movementData.endPoint = newPosition;
                
                replayManager.RecordMovement(ball1.name, movementData);
                
                MoveBall(movementData);
            }
            
            // TEST: REPLAY 
            if (Input.GetKeyUp(KeyCode.R))
            {
                switch (replayManager.state)
                {
                    case ReplayManager.ReplayState.Inactive:
                        replayManager.BeginReplay(ball1.name);
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

        // TEST: BALL
        private void MoveBall(MovementData movementData)
        {
            ball1.transform.position = Vector3.Lerp(movementData.startPoint, movementData.endPoint, Time.deltaTime);

        }

        private void HandleBeginReplay(MovementData movementData)
        {
            Debug.Log("Begin Replay!");
            MoveBall(movementData);
        }
        
        private void HandlePlayNextMovement(MovementData movementData)
        {
            Debug.Log("Play Next Movement!");
            MoveBall(movementData);
        }
        
        private void HandleEndReplay()
        {
            Debug.Log("End Replay!");
        }
    }
}