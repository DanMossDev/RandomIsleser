using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace RandomIsleser
{
    public class DollyCameraAutoComplete : MonoBehaviour
    {
        [SerializeField] private float _speed = 1;
        [SerializeField] private bool _loop;
        [SerializeField] private bool _deactivateCameraOnComplete;

        private float _nodes;
        private CinemachineVirtualCamera _vcam;
        private CinemachineTrackedDolly _trackedDolly;
        
        public bool Wait = false;

        private void Awake()
        {
            _vcam = GetComponent<CinemachineVirtualCamera>();
            _trackedDolly = _vcam.GetCinemachineComponent<CinemachineTrackedDolly>();
            _nodes = _trackedDolly.m_Path.MaxPos;
        }

        private void FixedUpdate()
        {
            if (Wait)
                return;
            
            _trackedDolly.m_PathPosition += Time.deltaTime * _speed;

            if (!_loop && _trackedDolly.m_PathPosition >= _nodes)
            {
                if (_deactivateCameraOnComplete)
                    gameObject.SetActive(false);
                else
                    enabled = false;
            }
        }
    }
}
