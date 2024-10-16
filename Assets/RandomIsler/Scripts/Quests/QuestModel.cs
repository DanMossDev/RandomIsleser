using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "QuestModel", menuName = "RandomIsler/Quests/QuestModel")]
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
        
        public bool CanBeStarted => _prerequisiteQuest == null || _prerequisiteQuest.IsComplete;

        public static event Action<QuestModel> OnQuestStarted;
        public static event Action<QuestModel> OnQuestComplete;

        public void BeginQuest()
        {
            if (IsStarted)
                return;
            
            IsStarted = true;
            Objectives[0].StartObjective();
            OnQuestStarted?.Invoke(this);
        }
        
        private void CompleteQuest()
        {
            if (IsComplete)
                return;
            Debug.Log("Quest complete!");
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
                BeginQuest();
        }
        
        protected override void Cleanup()
        {
            base.Cleanup();

            IsStarted = false;
            IsComplete = false;
            ObjectiveIndex = 0;
            foreach (var obj in Objectives)
                obj.Owner = this;
            
            if (!SaveableObjectHelper.Instance.AllQuests.Contains(this))
                SaveableObjectHelper.Instance.AllQuests.Add(this);
        }

        public override void Load(SOData data)
        {
            var questData = data as QuestData;
            if (questData == null)
                return;
            
            IsStarted = questData.IsStarted;
            IsComplete = questData.IsComplete;
        }

        public override SOData GetData()
        {
            var data = new QuestData()
            {
                ID = ID,
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
}
