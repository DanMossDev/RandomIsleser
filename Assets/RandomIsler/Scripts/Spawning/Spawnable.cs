using UnityEngine;

namespace RandomIsleser
{
    public abstract class Spawnable : MonoBehaviour
    {
        [Header("Spawnable")]
        public string ObjectPoolKey;
        
        public virtual void OnSpawned(SpawnPoint spawner)
        {}
    }
}
