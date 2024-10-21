using UnityEngine;

namespace RandomIsleser
{
    public class SpawnEnemiesEvent : ScriptedEvent
    {
        [Space, Header("Spawn Enemies Event Settings")]
        [SerializeField] private SpawnPoint[] _onStartSpawnPoints;
        
        [SerializeField] private GameObject[] _onEndActivate;

        private int _enemyCount;

        public override void BeginEvent()
        {
            if (_alreadyCompletedBool.Value)
            {
                SpawnRewards();
                return;
            }
            
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

            SpawnRewards();
        }

        private void SpawnRewards()
        {
            foreach (var GO in _onEndActivate)
                GO.SetActive(true);
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
