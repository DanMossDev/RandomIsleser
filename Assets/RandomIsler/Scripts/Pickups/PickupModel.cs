using UnityEngine;

namespace RandomIsleser
{
    public class PickupModel : ScriptableObject
    {
        [Header("Pickup Settings")]
        public PickupType Type;
        
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
