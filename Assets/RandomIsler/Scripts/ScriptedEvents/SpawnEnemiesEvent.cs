using UnityEngine;

namespace RandomIsleser
{
    public class SpawnEnemiesEvent : ScriptedEvent
    {
        [Space, Header("Spawn Enemies Event Settings")]
        [SerializeField] private SpawnPoint[] _onStartSpawnPoints;
        [SerializeField] private SpawnPoint[] _onEndSpawnPoints;

        private int _enemyCount;

        public override void BeginEvent()
        {
            base.BeginEvent();

            foreach (var spawnPoint in _onStartSpawnPoints)
            {
                var GO = spawnPoint.Spawn();
                Enemy enemy = GO.GetComponent<Enemy>();
                if (enemy != null)
                {
                    _enemyCount++;
                    enemy.OnDeath += OnEnemyDeath;
                }
            }
        }

        public override void EndEvent()
        {
            base.EndEvent();

            foreach (var spawnPoint in _onEndSpawnPoints)
                spawnPoint.Spawn();
        }

        private void OnEnemyDeath(Enemy enemy)
        {
            _enemyCount--;
            enemy.OnDeath -= OnEnemyDeath;
            
            if (_enemyCount <= 0)
                EndEvent();
        }
    }
}
