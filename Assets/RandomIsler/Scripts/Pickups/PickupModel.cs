using UnityEngine;
using UnityEngine.Localization;

namespace RandomIsleser
{
    public class PickupModel : ScriptableObject
    {
        [Header("Pickup Settings")]
        public PickupType Type;
        public LocalizedString Name;
        public LocalizedString FlavourText;
        
        public virtual void PickUp()
        { }
    }

    public enum PickupType
    {
        Health,
        Currency,
        Collectible,
        Item
    }
}
