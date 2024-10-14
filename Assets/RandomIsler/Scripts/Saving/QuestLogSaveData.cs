using System;
using System.Collections.Generic;

namespace RandomIsleser
{
    [Serializable]
    public class QuestLogSaveData
    {
        public List<QuestData> CompletedQuests;
        public List<QuestData> StartedQuests;

        public void Initialise()
        {
            CompletedQuests = new List<QuestData>();
            StartedQuests = new List<QuestData>();
        }

        public void QuestStarted(QuestData quest)
        {
            StartedQuests.Add(quest);
        }

        public void QuestUpdated(QuestData quest)
        {
            if (StartedQuests.Contains(quest))
            {
                StartedQuests.Remove(quest);
                StartedQuests.Insert(0, quest);
            }
        }

        public void QuestCompleted(QuestData quest)
        {
            if (StartedQuests.Contains(quest))
                StartedQuests.Remove(quest);
            
            if (!CompletedQuests.Contains(quest))
                CompletedQuests.Insert(0, quest);
        }
    }
}
