using System;
using UnityEngine;

namespace RandomIsleser
{
    public class OceanController : MossUtils.MonoSingleton<OceanController>
    {
        [SerializeField] private float _waveAmplitude;
        [SerializeField] private float _waveFrequency;
        [SerializeField] private float _waveSpeed;

        private GameObject _ocean;

        [SerializeField] private Material _oceanMaterial;

        private void Update()
        {
            var model = new OceanModel()
            {
                WaveAmplitude = _waveAmplitude,
                WaveFrequency = _waveFrequency,
                WaveSpeed = _waveSpeed
            };
            SetOceanValues(model);
        }

        public void SetOceanValues(OceanModel model)
        {
            _oceanMaterial.SetFloat("_Wave_Amplitude", model.WaveAmplitude);
            _oceanMaterial.SetFloat("_Wave_Frequency", model.WaveFrequency);
            _oceanMaterial.SetFloat("_WaveSpeed", model.WaveSpeed);
        }

        public float GetHeightAtPosition(Vector3 worldPosition)
        {
            var posSum = worldPosition.x + worldPosition.z;
            var posDif = worldPosition.x - worldPosition.z;
            var t = Time.time * _waveSpeed;

            var sineAmoumnt = Mathf.Sin((posSum + t) * _waveFrequency) * _waveAmplitude;
            var cosineAmount = Mathf.Cos((posDif + t) * _waveFrequency) * _waveAmplitude; 
            return sineAmoumnt * cosineAmount + transform.position.y;
        }
    }

    public class OceanModel
    {
        public float WaveAmplitude;
        public float WaveFrequency;
        public float WaveSpeed;
    }
}
