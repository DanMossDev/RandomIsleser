using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Localization;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "ObjectiveModel", menuName = "RandomIsler/Quests/ObjectiveModel")]
    public class ObjectiveModel : SaveableObject
    {
        public QuestModel Owner;

        public bool IsStarted;
        public bool IsComplete;
        
        public LocalizedString ObjectiveName;
        public LocalizedString ObjectiveDescription;

        [SerializeField] private ObjectiveModel _prerequisiteObjective;
        public DialogueTree OnStartDialogue;
        public DialogueTree OnCompleteDialogue;
        
        [SerializedDictionary] public SerializedDictionary<NPCModel, DialogueTree> InProgressDialogue = new SerializedDictionary<NPCModel, DialogueTree>();

        [HideInInspector] public bool HasStartDialogue;
        [HideInInspector] public bool HasProgressDialogue;
        [HideInInspector] public bool HasCompleteDialogue;
        
        public bool CanBeStarted => (Owner.IsStarted || Owner.CanBeStarted) && (_prerequisiteObjective == null || _prerequisiteObjective.IsComplete);

        public void StartObjective()
        {
            if (IsStarted)
                return;
            if (!Owner.IsStarted)
                Owner.BeginQuest();
            
            IsStarted = true;
        }

        public void CompleteObjective()
        {
            if (IsComplete)
                return;
            
            IsComplete = true;
            Owner.ObjectiveCompleted(this);
        }
        
        protected override void Cleanup()
        {
            base.Cleanup();

            IsStarted = false;
            IsComplete = false;
            
            HasStartDialogue = OnStartDialogue != null;
            HasCompleteDialogue = OnCompleteDialogue != null;
            HasProgressDialogue = InProgressDialogue.Count > 0;
            
            if (!SaveableObjectHelper.Instance.AllObjectives.Contains(this))
                SaveableObjectHelper.Instance.AllObjectives.Add(this);
        }
        
        public override void Load(SOData data)
        {
            var objData = data as ObjectiveData;
            if (objData == null)
                return;
            IsStarted = objData.IsStarted;
            IsComplete = objData.IsComplete;
        }

        public override SOData GetData()
        {
            return new ObjectiveData()
            {
                ID = ID,
                IsStarted = IsStarted,
                IsComplete = IsComplete,
            };
        }
    }
}
