using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class QuestManager : MonoBehaviour
    {
        public List<QuestModel> InProgressQuests => Services.Instance.RuntimeSaveManager.LocalSaveData.QuestSaveData.InProgressQuestModels;
        
        private void Awake()
        {
            QuestModel.OnQuestStarted += QuestStarted;
            QuestModel.OnQuestComplete += QuestCompleted;
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
        
        private void QuestCompleted(QuestModel quest)
        {
            Services.Instance.RuntimeSaveManager.LocalSaveData.QuestSaveData.QuestCompleted(quest);
        }
    }
}
