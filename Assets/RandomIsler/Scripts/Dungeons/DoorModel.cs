using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "DoorModel", menuName = AssetMenuNames.Models + "DoorModel")]
    public class DoorModel : SaveableObject
    {
        public bool IsLocked;
        
        public bool IsBossDoor;

        [SerializeField] private SaveableBool _unlockCondition;
        [SerializeField] private bool _beginsLocked;

        public DoorController Controller;

        public bool TryUnlockDoor()
        {
            if (IsBossDoor)
            {
                IsLocked = false;
                return true;
            }

            if (_unlockCondition != null)
            {
                IsLocked = false;
                return _unlockCondition.Value;
            }

            if (DungeonController.Instance.DungeonModel.SmallKeys > 0)
            {
                IsLocked = false;
                DungeonController.Instance.DungeonModel.SmallKeys--;
                return true;
            }
            return false;
        }
        
        public override void Load(SOData data)
        {
            var doorData = data as DungeonDoorData;
            if (doorData == null)
                return;
            
            IsLocked = doorData.IsLocked;
        }

        public override SOData GetData()
        {
            return new DungeonDoorData()
            {
                ID = ID,
                IsLocked = IsLocked
            };
        }

        protected override void Cleanup()
        {
            base.Cleanup();

            IsLocked = _beginsLocked;
        }
    }
    
    public class DungeonDoorData : SOData
    {
	    public bool IsLocked;
	    public bool IsBossDoor;
    }
}
