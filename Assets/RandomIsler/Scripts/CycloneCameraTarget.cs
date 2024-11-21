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
        
        private void Update()
        {
            transform.position = _playerTransform.position;//Vector3.Lerp(transform.position, _playerTransform.position, Time.deltaTime);
        }

        public void SetRotation()
        {
            var eulers = _mainCameraTransform.eulerAngles;
            eulers.x = eulers.z = 0;
            transform.eulerAngles = eulers;
        }
    }
}
