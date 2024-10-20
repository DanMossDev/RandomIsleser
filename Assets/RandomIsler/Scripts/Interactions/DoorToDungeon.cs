using UnityEngine;

namespace RandomIsleser
{
    public class DoorToDungeon : Door
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
