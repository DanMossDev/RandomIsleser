using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "DropTableModel", menuName = AssetMenuNames.Models + "DropTableModel")]
    public class DropTableModel : ScriptableObject
    {
        [SerializeField] List<DropChance> _drops = new List<DropChance>();

        public bool TryGetRandomDrop(out Spawnable randomDrop)
        {
            float randomRoll = Random.Range(0f, 1f);

            foreach (var drop in _drops)
            {
                if (randomRoll <= drop.Chance)
                {
                    randomDrop = drop.Collectable.Prefab;
                    return true;
                }
                randomRoll -= drop.Chance;
            }
            
            randomDrop = null;
            return false;
        }
    }

    [System.Serializable]
    public class DropChance
    {
        public float Chance;
        public CollectableModel Collectable;
    }
}
