using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "CycloneJarEquippableModel", menuName = AssetMenuNames.EquippableModels + "CycloneJarModel")]
    public class CycloneJarModel : EquippableModel
    {
        [Space, Header("Cyclone Jar")]
        [Range(0,1)] public float MovementSpeedMultiplier = 0f;
        [Range(0,1)] public float RotationSpeedMultiplier = 0.25f;
        public float ChargeSpeed = 1;
        public float CooldownTime = 0.5f;
        public float SuctionForce = 10f;
        public float FireCooldown = 1f;
        public float FiringForce = 10f;
        public float SuckDistance = 10f;

        public float JumpChargeRate = 1f;
        public float JumpHeight = 4f;
    }
}
