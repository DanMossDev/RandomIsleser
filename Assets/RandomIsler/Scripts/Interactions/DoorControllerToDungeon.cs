using UnityEngine;

namespace RandomIsleser
{
    public class DoorControllerToDungeon : DoorController
    {
        [SerializeField] private DungeonModel _dungeonToEnter;
        public override async void Interact()
        {
            _doorAnimator.SetTrigger(Animations.OpenDoorHash);
            await PlayerController.Instance.MoveThroughDoorToTargetPosition(_entryPoint[0].position);
            
            _dungeonToEnter.LoadDungeonScene();
        }
    }
}
