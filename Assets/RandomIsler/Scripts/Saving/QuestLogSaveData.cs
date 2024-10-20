using System;
using System.Collections.Generic;
using MossUtils;
using Newtonsoft.Json;

namespace RandomIsleser
{
    [Serializable]
    public class QuestLogSaveData
    {
        public List<QuestData> CompletedQuests;
        public List<QuestData> StartedQuests;

        [JsonIgnore] public List<QuestModel> CompletedQuestModels = new List<QuestModel>();
        [JsonIgnore] public List<QuestModel> InProgressQuestModels = new List<QuestModel>();

        public void Initialise()
        {
            CompletedQuests = new List<QuestData>();
            StartedQuests = new List<QuestData>();
        }
        
        public void QuestStarted(QuestModel quest)
        {
            InProgressQuestModels.Add(quest);
            
            QuestStarted(quest.GetData() as QuestData);
        }

        public void QuestUpdated(QuestModel quest)
        {
            if (InProgressQuestModels.Contains(quest))
            {
                InProgressQuestModels.Remove(quest);
                InProgressQuestModels.Insert(0, quest);
            }
            
            QuestUpdated(quest.GetData() as QuestData);
        }

        public void QuestCompleted(QuestModel quest)
        {
            if (InProgressQuestModels.Contains(quest))
                InProgressQuestModels.Remove(quest);
            
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
            for (int i = 0; i < StartedQuests.Count; i++)
            {
                if (StartedQuests[i].ID == quest.ID)
                {
                    StartedQuests.RemoveAt(i);
                    StartedQuests.Insert(0, quest);
                }
            }
        }

        private void QuestCompleted(QuestData quest)
        {
            for (int i = 0; i < StartedQuests.Count; i++)
            {
                if (StartedQuests[i].ID == quest.ID)
                    StartedQuests.RemoveAt(i);
            }

            bool add = true;
            for (int i = 0; i < CompletedQuests.Count; i++)
            {
                if (CompletedQuests[i].ID == quest.ID)
                    add = false;
            }
            
            if (add)
                CompletedQuests.Insert(0, quest);
        }
    }
}
