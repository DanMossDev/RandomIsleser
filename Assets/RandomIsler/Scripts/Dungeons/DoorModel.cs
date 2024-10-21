using System;
using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "DoorModel", menuName = AssetMenuNames.Models + "DoorModel")]
    public class DoorModel : SaveableObject
    {
        public bool IsLocked;
        
        public bool IsBossDoor;

        public bool LockOnEnter;

        public bool IsTemporaryLocked;

        [SerializeField] private SaveableBool _unlockCondition;
        [SerializeField] private bool _beginsLocked;

        public bool ConditionMet => _unlockCondition != null && _unlockCondition.Value;
        [NonSerialized] public DoorController Controller;

        public void SubscribeConditions()
        {
            if (_unlockCondition == null || _unlockCondition.Value)
                return;

            _unlockCondition.OnValueChanged += UnlockDoor;
        }

        public bool TryUnlockDoor()
        {
            if (_unlockCondition != null)
                return false;
            
            if (IsBossDoor)
            {
                IsLocked = false;
                return true;
            }

            if (DungeonController.Instance.DungeonModel.SmallKeys > 0)
            {
                IsLocked = false;
                DungeonController.Instance.DungeonModel.SmallKeys--;
                return true;
            }
            return false;
        }

        public void TemporaryLock()
        {
            IsTemporaryLocked = true;
        }

        private void UnlockDoor(bool complete)
        {
            if (!complete)
                return;
            
            IsTemporaryLocked = false;
            IsLocked = false;
            Controller.DoorUnlocked();
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
            IsTemporaryLocked = false;
        }
    }
    
    public class DungeonDoorData : SOData
    {
	    public bool IsLocked;
	    public bool IsBossDoor;
    }
}
