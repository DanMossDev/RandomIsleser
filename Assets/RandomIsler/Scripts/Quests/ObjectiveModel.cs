using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Localization;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "ObjectiveModel", menuName = AssetMenuNames.Quests + "ObjectiveModel")]
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

        public RewardModel OnCompleteReward;
        
        [SerializedDictionary] public SerializedDictionary<NPCModel, DialogueTree> InProgressDialogue = new SerializedDictionary<NPCModel, DialogueTree>();

        [HideInInspector] public bool HasStartDialogue;
        [HideInInspector] public bool HasProgressDialogue;
        [HideInInspector] public bool HasCompleteDialogue;
        [HideInInspector] public bool HasReward;
        
        public bool CanBeStarted => (Owner.IsStarted || Owner.CanBeStarted) && (_prerequisiteObjective == null || _prerequisiteObjective.IsComplete);

        public void StartObjective()
        {
            if (IsStarted)
                return;
            if (!Owner.IsStarted)
                Owner.BeginQuest(false);
            
            IsStarted = true;
            Services.Instance.RuntimeSaveManager.LocalSaveData.QuestSaveData.QuestUpdated(Owner);
        }

        public void CompleteObjective()
        {
            if (IsComplete)
                return;
            
            IsComplete = true;
            if (HasReward)
                OnCompleteReward.UnlockReward();
            Owner.ObjectiveCompleted(this);
            
            Services.Instance.RuntimeSaveManager.LocalSaveData.QuestSaveData.QuestUpdated(Owner);
        }
        
        protected override void Cleanup()
        {
            base.Cleanup();

            IsStarted = false;
            IsComplete = false;
            
            HasStartDialogue = OnStartDialogue != null;
            HasCompleteDialogue = OnCompleteDialogue != null;
            HasProgressDialogue = InProgressDialogue.Count > 0;
            HasReward = OnCompleteReward != null;
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
                Name = name,
                IsStarted = IsStarted,
                IsComplete = IsComplete,
            };
        }
    }
    
    [Serializable]
    public class ObjectiveData : SOData
    {
	    public bool IsStarted = false;
	    public bool IsComplete = false;
    }
}
