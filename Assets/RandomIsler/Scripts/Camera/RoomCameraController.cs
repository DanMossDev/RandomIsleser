using Cinemachine;
using UnityEngine;

namespace RandomIsleser
{
    public class RoomCameraController : MonoBehaviour
    {
        [Header("Room Options")] 
        [SerializeField] private bool _useRoomCamera;
        
        [Space, Header("Serialized Components")]
        [SerializeField] private CinemachineVirtualCamera _roomCamera;
        [SerializeField] private Collider _cameraBounds;

        public void OnRoomEnter()
        {
            if (_useRoomCamera)
                _roomCamera.gameObject.SetActive(true);
            else
                Services.Instance.CameraManager.SetBounds(_cameraBounds);
        }

        public void OnRoomExit()
        {
            if (_useRoomCamera)
                _roomCamera.gameObject.SetActive(false);
        }
    }
}
