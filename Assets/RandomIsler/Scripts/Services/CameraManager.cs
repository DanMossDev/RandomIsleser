using System.Collections.Generic;
using Cinemachine;
using MossUtils;
using UnityEngine;

namespace RandomIsleser
{
    public class CameraManager : MonoSingleton<CameraManager>
    {
        [Header("Cameras")]
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private CinemachineFreeLook _followCamera;
        [SerializeField] private GameObject _aimCamera;
        [SerializeField] private GameObject _cycloneCamera;
        [SerializeField] private GameObject _solarPanelAimCamera;
        [SerializeField] private GameObject _boatCamera;
        [SerializeField] private GameObject _dialogueCamera;
        [SerializeField] private GameObject _doorCamera;
        [SerializeField] private GameObject _itemGetCamera;

        [Space, Header("Extensions")] 
        [SerializeField] private CinemachineTargetGroup _dialogueGroup;
        [SerializeField] private CinemachineConfiner _followCamConfiner;
        
        public Camera MainCamera => _mainCamera;
        public CinemachineFreeLook FollowCamera => _followCamera;
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

        public void SetAimCamera(bool value)
        {
            _aimCamera.SetActive(value);
        }

        public void SetCycloneCamera(bool value)
        {
            _cycloneCamera.SetActive(value);
        }

        public void SetSolarPanelAimCamera(bool value)
        {
            _solarPanelAimCamera.SetActive(value);
        }

        public void SetBoatCamera(bool value)
        {
            _boatCamera.SetActive(value);
        }

        public void SetDialogueCamera(bool value)
        {
            _dialogueGroup.transform.forward = _mainCamera.transform.forward;
            _dialogueCamera.SetActive(value);
        }

        public void SetDoorCamera(bool value)
        {
            _doorCamera.SetActive(value);
        }

        public void SetItemGetCamera(bool value)
        {
            _itemGetCamera.SetActive(value);
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
