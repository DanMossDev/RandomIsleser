using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace RandomIsleser
{
    public class NPCController : Entity, Interactable
    {
        [SerializeField] protected NPCModel _model;
        public NPCModel NPCModel => _model;

        private void Awake()
        {
            _model.Controller = this;
        }

        private void OnDestroy()
        {
            _model.Controller = null;
        }

        public virtual void Interact()
        {
            transform.DOLookAt(PlayerController.Instance.transform.position, 0.5f, AxisConstraint.Y);
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

        protected void PlayDialogue(DialogueTree dialogueTree)
        {
            Services.Instance.DialogueManager.BeginDialogueTree(dialogueTree);
        }
    }
}
