using System;
using UnityEngine;

namespace RandomIsleser
{
    public class OceanController : MossUtils.MonoSingleton<OceanController>
    {
        [SerializeField] private float _waveAmplitude;
        [SerializeField] private float _waveFrequency;
        [SerializeField] private float _waveSpeed;

        [SerializeField] private Material _oceanMaterial;
        
        private Collider[] _waterCollider = new Collider[1];

        // private void Update()
        // {
        //     var model = new OceanModel()
        //     {
        //         WaveAmplitude = _waveAmplitude,
        //         WaveFrequency = _waveFrequency,
        //         WaveSpeed = _waveSpeed
        //     };
        //     SetOceanValues(model);
        // }

        public void SetOceanValues(OceanModel model)
        {
            _oceanMaterial.SetFloat("_WaveAmplitude", model.WaveAmplitude);
            _oceanMaterial.SetFloat("_WaveFrequency", model.WaveFrequency);
            _oceanMaterial.SetFloat("_WaveSpeed", model.WaveSpeed);
        }

        public float GetHeightAtPosition(Vector3 worldPosition)
        {
            if (Physics.OverlapSphereNonAlloc(worldPosition, 1f, _waterCollider, 1 << ProjectLayers.WaterLayer, QueryTriggerInteraction.Collide) > 0)
            {
                return _waterCollider[0].bounds.max.y;
            }
            
            
            var posSum = worldPosition.x + worldPosition.z;
            var posDif = worldPosition.x - worldPosition.z;
            var t = Time.time * _waveSpeed;

            var sineAmount = Mathf.Sin((posSum + t) * _waveFrequency) * _waveAmplitude;
            var cosineAmount = Mathf.Cos((posDif + t) * _waveFrequency) * _waveAmplitude; 
            return sineAmount * cosineAmount + transform.position.y;
        }

        public bool CheckIfUnderWater(Vector3 worldPosition)
        {
            return GetHeightAtPosition(worldPosition) > worldPosition.y;
        }
    }

    public class OceanModel
    {
        public float WaveAmplitude;
        public float WaveFrequency;
        public float WaveSpeed;
    }
}
