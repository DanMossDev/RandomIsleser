using UnityEngine;
using UnityEngine.SceneManagement;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "DungeonModel", menuName = AssetMenuNames.Dungeons + "DungeonModel")]
    public class DungeonModel : SaveableObject
    {
        public int SmallKeys;
        public bool HasBossKey;
        public bool IsComplete;

        [SerializeField] private string _sceneName;

        public void LoadDungeonScene()
        {
            SceneTransitionManager.LoadScene(_sceneName);
        }

        public override void Load(SOData data)
        {
            var dungeonData = data as DungeonData;
            if (dungeonData == null)
                return;
            
            SmallKeys = dungeonData.SmallKeys;
        }

        public override SOData GetData()
        {
            return new DungeonData()
            {
                ID = ID,
                SmallKeys = SmallKeys,
                IsComplete = IsComplete,
                HasBossKey = HasBossKey
            };
        }
    }
    
    public class DungeonData : SOData
    {
	    public int SmallKeys;
	    public bool HasBossKey;
	    public bool IsComplete;
    }
}
