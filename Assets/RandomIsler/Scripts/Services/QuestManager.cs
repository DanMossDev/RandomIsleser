using UnityEngine;

namespace RandomIsleser
{
    public class QuestManager : MonoBehaviour
    {
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

        private void QuestStarted(QuestData quest)
        {
            Services.Instance.RuntimeSaveManager.LocalSaveData.QuestSaveData.QuestStarted(quest);
        }
        
        private void QuestUpdated(QuestData quest)
        {
            Services.Instance.RuntimeSaveManager.LocalSaveData.QuestSaveData.QuestUpdated(quest);
        }
        
        private void QuestCompleted(QuestData quest)
        {
            Services.Instance.RuntimeSaveManager.LocalSaveData.QuestSaveData.QuestCompleted(quest);
        }
    }
}
