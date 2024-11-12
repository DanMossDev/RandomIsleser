using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class QuestManager : MonoBehaviour
    {
        public List<QuestModel> InProgressQuests => RuntimeSaveManager.Instance.LocalSaveData.QuestSaveData.InProgressQuestModels;
        
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
            RuntimeSaveManager.Instance.LocalSaveData.QuestSaveData.QuestStarted(quest);
        }
        
        private void QuestCompleted(QuestModel quest)
        {
            RuntimeSaveManager.Instance.LocalSaveData.QuestSaveData.QuestCompleted(quest);
        }
    }
}
