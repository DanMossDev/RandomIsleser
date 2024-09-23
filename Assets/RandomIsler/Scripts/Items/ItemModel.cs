using UnityEngine;
using UnityEngine.Localization;

namespace RandomIsleser
{
    public class ItemModel : ScriptableObject
    {
        [SerializeField] private LocalizedString _itemName;
        
        [SerializeField] private GameObject _itemPrefab;
        
        public LocalizedString ItemName => _itemName;
        public GameObject ItemPrefab => _itemPrefab;
    }
}
