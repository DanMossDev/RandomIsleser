using UnityEngine;

namespace RandomIsleser
{
    public class ChanceSpawnPoint : SpawnPoint
    {
        [SerializeField] private DropTableModel _dropTable;

        private void Start()
        {
            TriggerSpawnChance();
        }

        public void TriggerSpawnChance()
        {
            if (_dropTable.TryGetRandomDrop(out _spawnable))
                Spawn();
        }
    }
}
