using System;
using System.Collections.Generic;
using MossUtils;
using Newtonsoft.Json;

namespace RandomIsleser
{
    [Serializable]
    public class QuestLogSaveData
    {
        [JsonIgnore] public List<QuestModel> CompletedQuestModels = new List<QuestModel>();
        [JsonIgnore] public List<QuestModel> InProgressQuestModels = new List<QuestModel>();

        public void Initialise()
        {
        }
        
        public void QuestStarted(QuestModel quest)
        {
            InProgressQuestModels.Add(quest);
        }

        public void QuestUpdated(QuestModel quest)
        {
            if (InProgressQuestModels.Contains(quest))
            {
                InProgressQuestModels.Remove(quest);
                InProgressQuestModels.Insert(0, quest);
            }
        }

        public void QuestCompleted(QuestModel quest)
        {
            if (InProgressQuestModels.Contains(quest))
                InProgressQuestModels.Remove(quest);
            
            if (!CompletedQuestModels.Contains(quest))
                CompletedQuestModels.Insert(0, quest);
        }
    }
}
