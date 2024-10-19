using Cinemachine;
using UnityEngine;

namespace RandomIsleser
{
    public class StationaryLookAtPlayerCamera : MonoBehaviour
    {
        private void OnEnable()
        {
            GetComponent<CinemachineVirtualCamera>().LookAt = PlayerController.Instance.transform;
        }
    }
}
