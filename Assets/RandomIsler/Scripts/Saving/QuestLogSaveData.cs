using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RandomIsleser
{
    [Serializable]
    public class QuestLogSaveData
    {
        public List<QuestData> CompletedQuests;
        public List<QuestData> StartedQuests;

        [JsonIgnore] public List<QuestModel> CompletedQuestModels = new List<QuestModel>();
        [JsonIgnore] public List<QuestModel> StartedQuestModels = new List<QuestModel>();

        public void Initialise()
        {
            CompletedQuests = new List<QuestData>();
            StartedQuests = new List<QuestData>();
        }
        
        public void QuestStarted(QuestModel quest)
        {
            StartedQuestModels.Add(quest);
            
            QuestStarted(quest.GetData() as QuestData);
        }

        public void QuestUpdated(QuestModel quest)
        {
            if (StartedQuestModels.Contains(quest))
            {
                StartedQuestModels.Remove(quest);
                StartedQuestModels.Insert(0, quest);
            }
            
            QuestUpdated(quest.GetData() as QuestData);
        }

        public void QuestCompleted(QuestModel quest)
        {
            if (StartedQuestModels.Contains(quest))
                StartedQuestModels.Remove(quest);
            
            if (!CompletedQuestModels.Contains(quest))
                CompletedQuestModels.Insert(0, quest);
            
            QuestCompleted(quest.GetData() as QuestData);
        }

        private void QuestStarted(QuestData quest)
        {
            StartedQuests.Add(quest);
        }

        private void QuestUpdated(QuestData quest)
        {
            if (StartedQuests.Contains(quest))
            {
                StartedQuests.Remove(quest);
                StartedQuests.Insert(0, quest);
            }
        }

        private void QuestCompleted(QuestData quest)
        {
            if (StartedQuests.Contains(quest))
                StartedQuests.Remove(quest);
            
            if (!CompletedQuests.Contains(quest))
                CompletedQuests.Insert(0, quest);
        }
    }
}
