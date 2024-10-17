using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class QuestManager : MonoBehaviour
    {
        private Dictionary<int, QuestModel> _quests = new Dictionary<int, QuestModel>();
        private Dictionary<int, ObjectiveModel> _objectives = new Dictionary<int, ObjectiveModel>();

        public List<QuestModel> InProgressQuests => Services.Instance.RuntimeSaveManager.LocalSaveData.QuestSaveData.InProgressQuestModels;
        
        private void Awake()
        {
            QuestModel.OnQuestStarted += QuestStarted;
            QuestModel.OnQuestComplete += QuestCompleted;

            foreach (var quest in SaveableObjectHelper.Instance.AllQuests)
                _quests.Add(quest.ID, quest);
            foreach (var objective in SaveableObjectHelper.Instance.AllObjectives)
                _objectives.Add(objective.ID, objective);
        }
        
        private void OnDestroy()
        {
            QuestModel.OnQuestStarted -= QuestStarted;
            QuestModel.OnQuestComplete -= QuestCompleted;
        }

        private void QuestStarted(QuestModel quest)
        {
            Services.Instance.RuntimeSaveManager.LocalSaveData.QuestSaveData.QuestStarted(quest);
        }
        
        private void QuestUpdated(QuestModel quest)
        {
            Services.Instance.RuntimeSaveManager.LocalSaveData.QuestSaveData.QuestUpdated(quest);
        }
        
        private void QuestCompleted(QuestModel quest)
        {
            Services.Instance.RuntimeSaveManager.LocalSaveData.QuestSaveData.QuestCompleted(quest);
        }

        public void LoadQuestData(List<QuestData> quests)
        {
            var data = Services.Instance.RuntimeSaveManager.LocalSaveData.QuestSaveData;
            foreach (var quest in quests)
            {
                _quests[quest.ID].Load(quest);
                
                if (quest.IsComplete)
                    data.CompletedQuestModels.Add(_quests[quest.ID]);
                else
                    data.InProgressQuestModels.Add(_quests[quest.ID]);
            
                foreach (var objective in quest.Objectives)
                {
                    _objectives[objective.ID].Load(objective);
                }
            }
        }
    }
}
