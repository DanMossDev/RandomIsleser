using Cinemachine;
using UnityEngine;

namespace RandomIsleser
{
    public class SetPlayerToCameraTargetsOnEnable : MonoBehaviour
    {
        [SerializeField] private bool _setLookAt;
        [SerializeField] private bool _setFollow;
        [SerializeField] private bool _isOrbitalCam;
        
        private CinemachineVirtualCamera _vcam;
        private CinemachineFreeLook _freeLook;
        
        private void OnEnable()
        {
            if (!_isOrbitalCam)
            {
                _vcam = GetComponent<CinemachineVirtualCamera>();
                if (_setLookAt)
                    _vcam.LookAt = PlayerController.Instance.transform;
                if (_setFollow)
                    _vcam.Follow = PlayerController.Instance.transform;
            }
            else
            {
                _freeLook = GetComponent<CinemachineFreeLook>();
                if (_setLookAt)
                    _freeLook.LookAt = PlayerController.Instance.CameraFollowTarget;
                if (_setFollow)
                    _freeLook.Follow = PlayerController.Instance.CameraFollowTarget;
            }
        }

        private void OnDisable()
        {
            if (!_isOrbitalCam)
            {
                _vcam.LookAt = null;
                _vcam.Follow = null;
            }
            else
            {
                _freeLook.LookAt = null;
                _freeLook.Follow = null;
            }
        }
    }
}
