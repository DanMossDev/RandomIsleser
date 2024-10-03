using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "FishingRodModel", menuName = "RandomIsler/Models/Equippables/FishingRodModel")]
    public class FishingRodModel : EquippableModel
    {
        public float Range = 30;
        public float AimTolerance = 1;

        public float WindUpTime = 0.5f;
        public float CastTime = 0.5f;
        public float GrappleSpeed = 15f;
        
        public LayerMask HitLayers;
        public LayerMask InteractLayer;
    }
}
