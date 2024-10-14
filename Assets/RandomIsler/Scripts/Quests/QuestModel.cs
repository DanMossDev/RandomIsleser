using System;
using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "QuestModel", menuName = "RandomIsler/Quests/QuestModel")]
    public class QuestModel : ScriptableObject
    {
        public QuestData QuestData;
        
        public static event Action<QuestData> OnQuestStarted;
        public static event Action<QuestData> OnQuestComplete;

        public void BeginQuest()
        {
            QuestData.IsStarted = true;
            OnQuestStarted?.Invoke(QuestData);
        }
        
        public void CompleteQuest()
        {
            QuestData.IsComplete = true;
            OnQuestComplete?.Invoke(QuestData);
        }
    }
}
