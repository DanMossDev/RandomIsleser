using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "AimableModel", menuName = "RandomIsler/Models/Items/AimableModel")]
    public class AimableModel : ScriptableObject
    {
        public float Range = 30;
        public float AimTolerance = 1;
        public LayerMask HitLayers;
        public LayerMask InteractLayer;

        //Fishing Rod
        public float WindUpTime = 0.5f;
        public float CastTime = 0.5f;
    }
}
