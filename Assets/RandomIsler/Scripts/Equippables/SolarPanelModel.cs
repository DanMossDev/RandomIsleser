using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "SolarPanelModel", menuName = AssetMenuNames.EquippableModels + "SolarPanelModel")]
    public class SolarPanelModel : EquippableModel
    {
        public float ChargeSpeed = 0.5f;
        public float FireSpeed = 1f;
        public float MaxDistance = 100f;
        public float RayWidth = 0.5f;
    }
}
