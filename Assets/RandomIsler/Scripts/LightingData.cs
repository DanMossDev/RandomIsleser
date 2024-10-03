using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "LightingData", menuName = "RandomIsler/Settings/LightingData")]
    public class LightingData : ScriptableObject
    {
        public Gradient AmbientColour;
        public Gradient DirectionalColour;
        public Gradient FogColour;
    }
}
