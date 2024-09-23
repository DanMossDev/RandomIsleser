using UnityEngine;

namespace RandomIsleser
{
    public class ItemSpawnPoint : MonoBehaviour
    {
        [SerializeField] private Obstacles _lockedBehind;
        
        public Obstacles LockedBehind => _lockedBehind;
        
        
        public ItemModel ItemToSpawn;
    }
}
