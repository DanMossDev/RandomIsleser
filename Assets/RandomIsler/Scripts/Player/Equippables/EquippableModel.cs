using UnityEngine;

namespace RandomIsleser
{
    public class EquippableModel : ScriptableObject
    {
        public int ItemIndex;
        public LayerMask HitLayers;
        public LayerMask InteractLayer;
    }
}
