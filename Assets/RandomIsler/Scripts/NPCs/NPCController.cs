using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class NPCController : Entity, Interactable
    {
        [SerializeField] private NPCModel _model;
        public NPCModel NPCModel => _model;

        public void Interact()
        {
            if (_model.StartObjectives.Count == 0 && _model.CompleteObjectives.Count == 0)
            {
                PlayRandomDialogue();
                return;
            }

            foreach (var obj in _model.CompleteObjectives)
            {
                if (obj.IsComplete)
                    continue;
                
                if (obj.IsStarted || obj.CanBeStarted)
                {
                    obj.CompleteObjective();
                    return;
                }
            }

            foreach (var obj in _model.StartObjectives)
            {
                if (obj.IsStarted)
                    continue;
                
                if (obj.CanBeStarted)
                {
                    obj.StartObjective();
                    return;
                }
            }
            
            
            PlayRandomDialogue();
        }

        private void PlayRandomDialogue()
        {
            var node = _model.CharacterDialogueModel.GetRandomDialogueNode();
            //TODO - Add a "PlayDialogue" method to dialogue manager, pass this in
        }
    }
}
