using System;
using System.Collections.Generic;

namespace RandomIsleser
{
    [Serializable]
    public class SaveSlotData
    {
        public QuestLogSaveData QuestSaveData;
        public InventoryData InventoryData;
        public Dictionary<int, SOData> ScriptableObjectData;

        public string LastSceneName;

        public string PlayerName;

        public void Initialise()
        {
            InventoryData = new InventoryData();
            QuestSaveData = new QuestLogSaveData();
            ScriptableObjectData = new Dictionary<int, SOData>();
            
            LastSceneName = string.Empty;
			
            InventoryData.Initialise();
            QuestSaveData.Initialise();
        }
    }
}
