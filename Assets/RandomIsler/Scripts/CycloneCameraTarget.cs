using MossUtils;
using UnityEngine;

namespace RandomIsleser
{
    public class CycloneCameraTarget : MonoSingleton<CycloneCameraTarget>
    {
        private Transform _playerTransform;
        private Transform _mainCameraTransform;
        
        private void Start()
        {
            _playerTransform = PlayerController.Instance.transform;
            _mainCameraTransform = PlayerController.Instance.MainCameraTransform;
        }
        
        private void FixedUpdate()
        {
            transform.position = _playerTransform.position;
        }

        public void SetRotation()
        {
            var eulers = _mainCameraTransform.eulerAngles;
            eulers.x = eulers.z = 0;
            transform.eulerAngles = eulers;
        }
    }
}
