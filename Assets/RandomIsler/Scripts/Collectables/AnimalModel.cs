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
    }
}
