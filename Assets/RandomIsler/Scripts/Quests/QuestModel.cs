using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "QuestModel", menuName = AssetMenuNames.Quests + "QuestModel")]
    public class QuestModel : SaveableObject
    {
        public LocalizedString QuestName;
        public LocalizedString QuestDescription;

        public LocalizedString CurrentObjectiveName => Objectives[ObjectiveIndex].ObjectiveName;
        public LocalizedString CurrentObjectiveDescription => Objectives[ObjectiveIndex].ObjectiveDescription;

        public bool IsStarted;
        public bool IsComplete;

        public int ObjectiveIndex = 0;

        [SerializeField] private QuestModel _prerequisiteQuest;

        public List<ObjectiveModel> Objectives;
        public ObjectiveModel LastCompletedObjective => Objectives[ObjectiveIndex - 1];
        public ObjectiveModel CurrentObjective => Objectives[ObjectiveIndex];
        
        public bool CanBeStarted => _prerequisiteQuest == null || _prerequisiteQuest.IsComplete;

        public static event Action<QuestModel> OnQuestStarted;
        public static event Action<QuestModel> OnQuestUpdated;
        public static event Action<QuestModel> OnQuestComplete;

        public void BeginQuest(bool beginFirstObjective = true)
        {
            if (IsStarted)
                return;
            
            IsStarted = true;
            if (beginFirstObjective)
                Objectives[0].StartObjective();
            
            OnQuestStarted?.Invoke(this);
        }
        
        private void CompleteQuest()
        {
            if (IsComplete)
                return;
            
            IsComplete = true;
            OnQuestComplete?.Invoke(this);
        }

        public void ObjectiveCompleted(ObjectiveModel objective)
        {
            ObjectiveIndex = Objectives.IndexOf(objective) + 1;
            if (ObjectiveIndex >= Objectives.Count)
            {
                CompleteQuest();
                return;
            }

            Objectives[ObjectiveIndex].StartObjective();
            if (!IsStarted)
                BeginQuest(false);
            else
                OnQuestUpdated?.Invoke(this);
        }
        
        protected override void Cleanup()
        {
            base.Cleanup();

            IsStarted = false;
            IsComplete = false;
            ObjectiveIndex = 0;
            foreach (var obj in Objectives)
                obj.Owner = this;
        }

        public override void Load(SOData data)
        {
            var questData = data as QuestData;
            if (questData == null)
                return;
            
            IsStarted = questData.IsStarted;
            IsComplete = questData.IsComplete;
            ObjectiveIndex = questData.ObjectiveIndex;
            
            if (IsComplete)
                RuntimeSaveManager.Instance.LocalSaveData.QuestSaveData.CompletedQuestModels.Add(this);
            else if (IsStarted)
                RuntimeSaveManager.Instance.LocalSaveData.QuestSaveData.InProgressQuestModels.Add(this);
        }

        public override SOData GetData()
        {
            var data = new QuestData()
            {
                ID = ID,
                Name = name,
                IsStarted = IsStarted,
                IsComplete = IsComplete, 
                ObjectiveIndex = ObjectiveIndex,
                Objectives = new List<ObjectiveData>()
            };
            
            foreach (var objective in Objectives)
            {
                data.Objectives.Add(objective.GetData() as ObjectiveData);
            }

            return data;
        }
    }
    
    [Serializable]
    public class QuestData : SOData
    {
	    public bool IsStarted = false;
	    public bool IsComplete = false;

	    public int ObjectiveIndex = 0;

	    public List<ObjectiveData> Objectives;
    }
}
