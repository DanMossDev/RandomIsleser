using System;
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
        
		#region Properties
		[JsonIgnore] public bool ExampleCheckBool => ExampleFlagData.HasFlag(SaveDataFlags.ExampleOne);
		#endregion

		public void InitialiseSaveData()
		{
			ExampleFlagData = SaveDataFlags.None;
			InventoryData = new InventoryData();
			QuestSaveData = new QuestLogSaveData();
			
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