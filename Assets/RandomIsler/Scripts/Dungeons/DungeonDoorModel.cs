using UnityEngine;

namespace RandomIsleser
{
    public class DungeonDoorModel : SaveableObject
    {
        public bool IsLocked;
        
        public bool IsBossDoor;
        [SerializeField] private DungeonModel _owner;
        [SerializeField] private bool _beginsLocked;

        public bool CanBeOpened()
        {
            return (IsBossDoor && _owner.HasBossKey ) || (!IsBossDoor && _owner.SmallKeys > 0);
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
