using Cinemachine;
using UnityEngine;

namespace RandomIsleser
{
    public class StationaryLookAtPlayerCamera : MonoBehaviour
    {
        private Transform _lookAt;
        private void OnEnable()
        {
            GetComponent<CinemachineVirtualCamera>().LookAt = PlayerController.Instance.transform;
        }
    }
}
