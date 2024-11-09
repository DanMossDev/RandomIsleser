using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "CollectableSettingsModel", menuName = AssetMenuNames.Settings + "CollectableSettingsModel")]
    public class CollectableSettingsModel : ScriptableObject
    {
        [SerializeField] private float _rareAnimalChance = 0.01f;

        public int GetAnimalRarityLevel(float luckMultiplier = 1)
        {
            float rareChance = _rareAnimalChance * luckMultiplier;

            float roll = Random.Range(0f, 1f);

            if (roll < rareChance)
                return 1;
            
            return 0;
        }
    }
}
