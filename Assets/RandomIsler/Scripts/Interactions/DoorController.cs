using UnityEngine;

namespace RandomIsleser
{
    public class DoorController : MonoBehaviour, Interactable
    {
        [SerializeField] protected DoorModel _model;
        
        [SerializeField] protected Transform[] _entryPoint;
        [SerializeField] private RoomCameraController[] _roomCameraControllers;

        [SerializeField] protected Animator _doorAnimator;

        protected virtual void Awake()
        {
            if (_model != null)
            {
                _model.Controller = this;
                _model.SubscribeConditions();
            }
        }
        
        public virtual void Interact()
        {
            bool hasModel = _model != null;

            if (hasModel && _model.IsTemporaryLocked)
            {
                //Maybe some SFX here too
                return;
            }
            
            if (hasModel && _model.LockOnEnter && !_model.ConditionMet && !_model.IsTemporaryLocked)
            {
                _model.TemporaryLock();
            }
            
            if (hasModel && _model.IsLocked)
            {
                if (!_model.TryUnlockDoor())
                {
                    //Play some kind of "door locked" sfx/anims
                    return;
                }
                DoorUnlocked();
            }
            
            _doorAnimator.SetTrigger(Animations.OpenDoorHash);
            var playerPos = PlayerController.Instance.transform.position;
            var point0Dist = Vector3.SqrMagnitude(playerPos - _entryPoint[0].position);
            var point1Dist = Vector3.SqrMagnitude(playerPos - _entryPoint[1].position);

            var currentIndex = point0Dist < point1Dist ? 0 : 1;
            var targetIndex = point0Dist < point1Dist ? 1 : 0;

            _ = PlayerController.Instance.MoveThroughDoorToTargetPosition(_entryPoint[targetIndex].position);
            
            if (_roomCameraControllers[currentIndex] != null)
                _roomCameraControllers[currentIndex].OnRoomExit();
            if (_roomCameraControllers[targetIndex] != null)
                _roomCameraControllers[targetIndex].OnRoomEnter();
        }

        public void DoorUnlocked()
        {
            //Show some unlock stuff
        }
    }
}
