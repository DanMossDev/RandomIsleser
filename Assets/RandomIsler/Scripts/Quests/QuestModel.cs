using System;
using UnityEngine;
using UnityEngine.Localization;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "QuestModel", menuName = "RandomIsler/Quests/QuestModel")]
    public class QuestModel : ScriptableObject
    {
        public QuestData QuestData;
        
        public static event Action<LocalizedString> OnQuestComplete;

        public void CompleteQuest()
        {
            QuestData.IsComplete = true;
            OnQuestComplete?.Invoke(QuestData.QuestName);
        }
    }
}
