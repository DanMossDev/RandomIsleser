using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace RandomIsleser
{
    public class CameraManager : MonoBehaviour
    {
        [Header("Cameras")]
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private CinemachineFreeLook _followCamera;
        [SerializeField] private GameObject _aimCamera;
        [SerializeField] private GameObject _cycloneCamera;
        [SerializeField] private GameObject _boatCamera;
        [SerializeField] private GameObject _dialogueCamera;
        [SerializeField] private CinemachineTargetGroup _dialogueGroup;

        [Space, Header("Extensions")] 
        [SerializeField] private CinemachineConfiner _followCamConfiner;
        
        public GameObject AimCamera => _aimCamera;

        private void DisableAll()
        {
            _aimCamera.SetActive(false);
            _cycloneCamera.SetActive(false);
            _cycloneCamera.SetActive(false);
            _boatCamera.SetActive(false);
            _dialogueCamera.SetActive(false);
        }

        public void SetDefaultCamera()
        {
            DisableAll();
        }

        public void SetBounds(Collider bounds)
        {
            _followCamConfiner.m_BoundingVolume = bounds;
        }

        public void SetAimCamera()
        {
            DisableAll();
            _aimCamera.SetActive(true);
        }

        public void SetCycloneCamera()
        {
            DisableAll();
            _cycloneCamera.SetActive(true);
        }

        public void SetBoatCamera()
        {
            DisableAll();
            _boatCamera.SetActive(true);
        }

        public void SetDialogueCamera()
        {
            DisableAll();
            _dialogueGroup.transform.forward = _mainCamera.transform.forward;
            _dialogueCamera.SetActive(true);
        }

        public void SetParticipants(List<NPCModel> participants)
        {
            foreach (var participant in participants)
                _dialogueGroup.AddMember(participant.Controller.transform, 1, 0);
        }

        public async void RemoveParticipants(List<NPCModel> participants)
        {
            await System.Threading.Tasks.Task.Delay(1000); //TODO - consider finding a way not to delay here
            foreach (var participant in participants)
                _dialogueGroup.RemoveMember(participant.Controller.transform);
        }

        public void SetCurrentSpeaker(NPCController participant)
        {
            for (int i = 0; i < _dialogueGroup.m_Targets.Length; i++)
            {
                var target = _dialogueGroup.m_Targets[i];
                target.weight = target.target == participant.transform ? 2 : 1;
            }
        }
    }
}
