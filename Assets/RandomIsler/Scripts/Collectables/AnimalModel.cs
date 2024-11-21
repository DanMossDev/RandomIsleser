using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "AnimalModel", menuName = AssetMenuNames.Models + "AnimalModel")]
    public class AnimalModel : CollectableModel
    {
        [Space, Header("Animal")]
        public float WanderSpeed = 1;
        public float FleeSpeed = 2;
        public float TurnSpeed = 360;
        
        public float FleeRange = 10;
        public float WanderRadius = 20;
        public float LureRadius = 10;

        [Range(0,1)] public float LureResistance = 0.95f;

        public float StunTime = 1;
        public float PickupShowTime = 1;

        public bool CollectOnContact = true;
        public bool CanBePulled = false;

        public AnimalFlags Species;

        public float MinBuoyancy = 5;
        public float MaxBuoyancy = 20;

        public float EscapeTime = 5;
    }
}
