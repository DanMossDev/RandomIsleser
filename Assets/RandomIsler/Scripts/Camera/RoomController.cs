using Cinemachine;
using UnityEngine;

namespace RandomIsleser
{
    public class RoomController : MonoBehaviour
    {
        [Header("Room Settings")] 
        [SerializeField] private ScriptedEvent[] _onEnterRoomEvents;
        [SerializeField] private SpawnPoint[] _spawnPoints;
        
        [Header("Camera Options")] 
        [SerializeField] private bool _useRoomCamera;
        [SerializeField] private CinemachineVirtualCamera _roomCamera;
        [SerializeField] private Collider _cameraBounds;
        

        public void OnRoomEnter()
        {
            foreach (var enterEvent in _onEnterRoomEvents)
                enterEvent.BeginEvent();

            foreach (var spawnPoint in _spawnPoints)
                spawnPoint.Spawn();
            
            SetupRoomCamera();
        }

        public void OnRoomExit()
        {
            if (_useRoomCamera)
                _roomCamera.gameObject.SetActive(false);
            
            foreach (var spawnPoint in _spawnPoints)
                spawnPoint.Despawn();
        }

        private void SetupRoomCamera()
        {
            if (_useRoomCamera)
            {
                _roomCamera.gameObject.SetActive(true);
                PlayerController.Instance.IncomingCameraBounds = null;
            }
            else
                PlayerController.Instance.IncomingCameraBounds = _cameraBounds;
        }
    }
}
