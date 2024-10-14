using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "QuestModel", menuName = "RandomIsler/Quests/QuestModel")]
    public class QuestModel : ScriptableObject
    {
        public int ID;
        
        public LocalizedString QuestName;
        public LocalizedString QuestDescription;

        public LocalizedString CurrentObjectiveName => Objectives[ObjectiveIndex].ObjectiveName;
        public LocalizedString CurrentObjectiveDescription => Objectives[ObjectiveIndex].ObjectiveDescription;

        public bool IsStarted;
        public bool IsComplete;

        public int ObjectiveIndex = 0;

        public List<ObjectiveModel> Objectives;

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
            
            if (!IsStarted)
                BeginQuest();
        }
        
        private void OnValidate()
        {
            if (ID == 0)
                ID = Guid.NewGuid().GetHashCode();

            IsStarted = false;
            IsComplete = false;
            ObjectiveIndex = 0;
        }

        public void Load(QuestData data)
        {
            IsStarted = data.IsStarted;
            IsComplete = data.IsComplete;
        }

        public QuestData GetData()
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
                data.Objectives.Add(objective.GetData());
            }

            return data;
        }
    }
}
