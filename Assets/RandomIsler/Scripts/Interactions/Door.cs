using UnityEngine;

namespace RandomIsleser
{
    public class Door : MonoBehaviour, Interactable
    {
        [SerializeField] protected Transform[] _entryPoint;
        [SerializeField] private RoomCameraController[] _roomCameraControllers;

        [SerializeField] protected Animator _doorAnimator;
        
        public virtual void Interact()
        {
            _doorAnimator.SetTrigger(Animations.OpenDoorHash);
            var playerPos = PlayerController.Instance.transform.position;
            var point0Dist = Vector3.SqrMagnitude(playerPos - _entryPoint[0].position);
            var point1Dist = Vector3.SqrMagnitude(playerPos - _entryPoint[1].position);

            var currentIndex = point0Dist < point1Dist ? 0 : 1;
            var targetIndex = point0Dist < point1Dist ? 1 : 0;

            PlayerController.Instance.MoveThroughDoorToTargetPosition(_entryPoint[targetIndex].position);
            
            if (_roomCameraControllers[currentIndex] != null)
                _roomCameraControllers[currentIndex].OnRoomExit();
            if (_roomCameraControllers[targetIndex] != null)
                _roomCameraControllers[targetIndex].OnRoomEnter();
        }
    }
}
