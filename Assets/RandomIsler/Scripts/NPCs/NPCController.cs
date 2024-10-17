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
            foreach (var obj in _model.CompleteObjectives)
            {
                if (obj.IsComplete)
                    continue;
                
                if (obj.IsStarted || obj.CanBeStarted)
                {
                    obj.CompleteObjective();
                    
                    if (obj.HasCompleteDialogue)
                        PlayDialogue(obj.OnCompleteDialogue);
                    return;
                }
            }

            foreach (var obj in _model.StartObjectives)
            {
                if (obj.IsStarted)
                {
                    continue;
                }
                
                if (obj.CanBeStarted)
                {
                    obj.StartObjective();

                    
                    if (obj.HasStartDialogue)
                        PlayDialogue(obj.OnStartDialogue);
                    return;
                }
            }
            
            var backupDialogue = _model.CharacterDialogueModel.GetRandomDialogueTree();
            foreach (var quest in Services.Instance.QuestManager.InProgressQuests)
            {
                if (quest.CurrentObjective.InProgressDialogue.TryGetValue(_model, out backupDialogue))
                    break;
            }
                
            PlayDialogue(backupDialogue);
        }

        private void PlayDialogue(DialogueTree dialogueTree)
        {
            Services.Instance.DialogueManager.BeginDialogueTree(dialogueTree);
        }
    }
}
