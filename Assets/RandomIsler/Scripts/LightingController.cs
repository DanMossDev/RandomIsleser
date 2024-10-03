using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    [ExecuteAlways]
    public class LightingController : MonoBehaviour
    {
        [SerializeField] private Light _directionalLight;
        [SerializeField] private LightingData _lightingData;
        [SerializeField, Range(0,24)] private float _timeOfDay;

        private void OnValidate()
        {
            if (_directionalLight != null)
                return;

            if (RenderSettings.sun != null)
                _directionalLight = RenderSettings.sun;
        }

        private void Update()
        {
            UpdateLighting((_timeOfDay % 24) / 24f);
        }

        private void UpdateLighting(float timeRatio)
        {
            RenderSettings.ambientLight = _lightingData.AmbientColour.Evaluate(timeRatio);
            RenderSettings.fogColor = _lightingData.FogColour.Evaluate(timeRatio);
            
            _directionalLight.color = _lightingData.DirectionalColour.Evaluate(timeRatio);
            _directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timeRatio * 360) - 90, 170, 0));
        }
    }
}
