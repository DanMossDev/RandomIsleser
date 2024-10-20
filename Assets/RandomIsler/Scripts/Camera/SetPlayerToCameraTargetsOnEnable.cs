using Cinemachine;
using UnityEngine;

namespace RandomIsleser
{
    public class SetPlayerToCameraTargetsOnEnable : MonoBehaviour
    {
        [SerializeField] private bool _setLookAt;
        [SerializeField] private bool _setFollow;
        [SerializeField] private bool _isOrbitalCam;
        
        private void OnEnable()
        {
            if (!_isOrbitalCam)
            {
                if (_setLookAt)
                    GetComponent<CinemachineVirtualCamera>().LookAt = PlayerController.Instance.transform;
                if (_setFollow)
                    GetComponent<CinemachineVirtualCamera>().Follow = PlayerController.Instance.transform;
            }
            else
            {
                if (_setLookAt)
                    GetComponent<CinemachineFreeLook>().LookAt = PlayerController.Instance.CameraFollowTarget;
                if (_setFollow)
                    GetComponent<CinemachineFreeLook>().Follow = PlayerController.Instance.CameraFollowTarget;
            }
        }
    }
}
