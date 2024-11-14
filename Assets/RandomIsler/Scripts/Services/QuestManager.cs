using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class QuestManager : MonoBehaviour
    {
        public List<QuestModel> InProgressQuests => RuntimeSaveManager.Instance.CurrentSaveSlot.QuestSaveData.InProgressQuestModels;
        
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
            RuntimeSaveManager.Instance.CurrentSaveSlot.QuestSaveData.QuestStarted(quest);
        }
        
        private void QuestCompleted(QuestModel quest)
        {
            RuntimeSaveManager.Instance.CurrentSaveSlot.QuestSaveData.QuestCompleted(quest);
        }
    }
}
