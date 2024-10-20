using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RandomIsleser
{
    public class DialogueManager : MonoBehaviour
    {
        private Dictionary<int, DialogueTree> _dialogueTrees = new Dictionary<int, DialogueTree>();
        
        private DialogueTree _currentDialogueTree;
        
        private DialogueUI _dialogueUI;
        private UIManager _uiManager;
        
        private void Awake()
        {
            foreach (var dialogueTree in SaveableObjectHelper.Instance.AllDialogueTrees)
                _dialogueTrees.Add(dialogueTree.ID, dialogueTree);
        }

        private void Start()
        {
            _uiManager = Services.Instance.UIManager;
            _dialogueUI = _uiManager.DialogueUI;
        }

        private async void SubscribeDialogueControls()
        {
            await Task.Yield();
            InputManager.AcceptInput += OnContinue;
            InputManager.BackInput += OnBack;
        }

        private void UnsubscribeDialogueControls()
        {
            InputManager.AcceptInput -= OnContinue;
            InputManager.BackInput -= OnBack;
        }
        
        public void LoadDialogueData(List<DialogueData> dialogue)
        {
            foreach (var tree in dialogue)
            {
                _dialogueTrees[tree.ID].Load(tree);
            }
        }

        public void BeginDialogueTree(DialogueTree dialogueTree)
        {
            _currentDialogueTree = dialogueTree;
            PlayerController.Instance.UnsubscribeControls();
            CameraManager.Instance.SetParticipants(dialogueTree.Participants);
            CameraManager.Instance.SetDialogueCamera(true);
            _uiManager.BeginDialogue(dialogueTree.GetFirstDialogueNode());
            
            SubscribeDialogueControls();
        }

        private void EndDialogue()
        {
            _uiManager.EndDialogue();
            CameraManager.Instance.SetDialogueCamera(false);
            CameraManager.Instance.RemoveParticipants(_currentDialogueTree.Participants);
            _currentDialogueTree = null;
            UnsubscribeDialogueControls();
            PlayerController.Instance.SubscribeControls();
        }

        private void OnBack()
        {
            OnContinue();
        }
        
        private void OnContinue()
        {
            if (!_dialogueUI.IsFinishedDisplaying)
            {
                _uiManager.SkipDialogue();
                return;
            }

            if (_dialogueUI.CurrentNode.IsEnd)
                EndDialogue();
            else
                _uiManager.ContinueDialogue();
                
        }
    }
}
