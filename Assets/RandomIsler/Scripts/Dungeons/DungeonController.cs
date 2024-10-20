using MossUtils;
using UnityEngine;

namespace RandomIsleser
{
    public class DungeonController : MonoSingleton<DungeonController>
    {
        [SerializeField] private DungeonModel _dungeonModel;
        
        public DungeonModel DungeonModel => _dungeonModel;

        public void AddKey(KeyRewardTypes type)
        {
	        if (type == KeyRewardTypes.Normal)
		        _dungeonModel.SmallKeys++;
	        else if (type == KeyRewardTypes.Big)
		        _dungeonModel.HasBossKey = true;
        }
    }
}
