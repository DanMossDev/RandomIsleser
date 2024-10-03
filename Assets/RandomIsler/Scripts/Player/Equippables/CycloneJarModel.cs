using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "CycloneJarEquippableModel", menuName = "RandomIsler/Models/Equippables/CycloneJarModel")]
    public class CycloneJarModel : EquippableModel
    {
        [Range(0,1)] public float MovementSpeedMultiplier = 0f;
        [Range(0,1)] public float RotationSpeedMultiplier = 0.25f;
        public float ChargeSpeed = 1;
        public float CooldownTime = 0.5f;
    }
}
