using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RandomIsleser;

namespace MossUtils
{
	[Serializable]
	public class SaveData
	{
		public QuestLogSaveData QuestSaveData;
		public InventoryData InventoryData;
		public SaveDataFlags ExampleFlagData;
		
		public Dictionary<int, SOData> ScriptableObjectData;
        
		#region Properties
		[JsonIgnore] public bool ExampleCheckBool => ExampleFlagData.HasFlag(SaveDataFlags.ExampleOne);
		#endregion

		public void InitialiseSaveData()
		{
			ExampleFlagData = SaveDataFlags.None;
			InventoryData = new InventoryData();
			QuestSaveData = new QuestLogSaveData();
			ScriptableObjectData = new Dictionary<int, SOData>();
			
			InventoryData.Initialise();
			QuestSaveData.Initialise();
		}

		public void SetFlags(SaveDataFlags flag, bool value)
		{
			if (value)
				ExampleFlagData |= flag;
			else
				ExampleFlagData &= ~flag;
		}
	}
}