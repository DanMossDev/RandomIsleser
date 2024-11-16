using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

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
        public float TimePlayedSeconds;
        
        [JsonIgnore] public int DisplaySlot => Slot + 1;
        [JsonIgnore] public TimeSpan TimePlayed => TimeSpan.FromSeconds(TimePlayedSeconds);
        [JsonIgnore] public bool DataExists => !string.IsNullOrEmpty(PlayerName);
        [JsonIgnore] private float _lastSaveTime;

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

        public void ResetRuntimeValues()
        {
            QuestSaveData.InProgressQuestModels.Clear();
            QuestSaveData.CompletedQuestModels.Clear();
        }

        public void Loaded()
        {
            _lastSaveTime = Time.time;
        }

        public void Saved()
        {
            if (_lastSaveTime != 0)
                TimePlayedSeconds += Time.time - _lastSaveTime;
            
            _lastSaveTime = Time.time;
        }
    }
}
