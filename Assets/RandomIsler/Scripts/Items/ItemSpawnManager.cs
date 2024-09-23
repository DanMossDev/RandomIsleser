using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RandomIsleser
{
    public class ItemSpawnManager : MonoBehaviour
    {
        //Items
        [SerializeField] private List<ProgressItemModel> _progressItems;
        [SerializeField] private List<ItemModel> _winConditionItems;
        [SerializeField] private List<ItemModel> _metaProgressionItems;
        [SerializeField] private List<ItemModel> _miscItems;
        
        //Spawn Points
        [SerializeField] private List<ItemSpawnPoint> _worldSpawnPoints;

        private List<ProgressItemModel> _itemsToSpawn;
        private List<ItemSpawnPoint> _spawnPoints;

        private List<ItemSpawnPoint> _spawnedPoints;
        private Obstacles _accessibleObstacles;

        private void Start()
        {
            RandomiseSpawns();
            
            while (!ValidateRandomisation())
                RandomiseSpawns();
        }

        private void RandomiseSpawns()
        {
            _spawnedPoints = new List<ItemSpawnPoint>();
            _accessibleObstacles = 0;
            
            _itemsToSpawn =  new List<ProgressItemModel>(_progressItems);
            _spawnPoints = new List<ItemSpawnPoint>(_worldSpawnPoints);
            foreach (var point in _spawnPoints)
                point.ItemToSpawn = null;

            while (_itemsToSpawn.Count > 0)
            {
                int spawnIndex = Random.Range(0, _spawnPoints.Count);
                int itemIndex = Random.Range(0, _itemsToSpawn.Count);
                
                _spawnPoints[spawnIndex].ItemToSpawn = _itemsToSpawn[itemIndex];
                _spawnedPoints.Add(_spawnPoints[spawnIndex]);
                _spawnPoints.RemoveAt(spawnIndex);
                _itemsToSpawn.RemoveAt(itemIndex);
            }
        }

        private bool ValidateRandomisation()
        {
            bool accessChanged = true;
            while (accessChanged && _spawnedPoints.Count > 0)
            {
                List<ItemSpawnPoint> newSpawnPoints = new List<ItemSpawnPoint>(_spawnedPoints);
                
                Obstacles accessible = _accessibleObstacles;
                foreach (var point in _spawnedPoints)
                {
                    if ((_accessibleObstacles & point.LockedBehind) == point.LockedBehind)
                    {
                        Debug.Log($"{_accessibleObstacles} contains all of {point.LockedBehind}");
                        _accessibleObstacles |= ((ProgressItemModel)point.ItemToSpawn).CanNavigate;
                        newSpawnPoints.Remove(point);
                    }
                }
                _spawnedPoints = newSpawnPoints;
                accessChanged = accessible != _accessibleObstacles;
            }
            
            return _spawnedPoints.Count == 0;
        }
    }
}
