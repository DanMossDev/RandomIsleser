using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RandomIsleser
{
    [Serializable]
    public class SaveSlotData
    {
        public int Slot;
        public QuestLogSaveData QuestSaveData;
        public InventoryData InventoryData;
        public Dictionary<int, SOData> ScriptableObjectData;

        public string LastSceneName;

        public string PlayerName;

        public long TimePlayedTicks;
        
        [JsonIgnore] public TimeSpan TimePlayed => TimeSpan.FromTicks(TimePlayedTicks);
        [JsonIgnore] public bool DataExists => !string.IsNullOrEmpty(PlayerName);
        [JsonIgnore] private long _lastSaveTime;

        public void Initialise(int slot)
        {
            Slot = slot;
            InventoryData = new InventoryData();
            QuestSaveData = new QuestLogSaveData();
            ScriptableObjectData = new Dictionary<int, SOData>();
            
            LastSceneName = string.Empty;
			
            InventoryData.Initialise();
            QuestSaveData.Initialise();
        }
    }
}
