using System;
using System.Collections.Generic;
using RandomIsleser;

namespace MossUtils
{
	[Serializable]
	public class SaveData
	{
		public QuestLogSaveData QuestSaveData;
		public InventoryData InventoryData;
		
		
		public AnimalFlags AnimalData;
		public FishFlags FishData;
		public BirdFlags BirdData;
		
		public Dictionary<int, SOData> ScriptableObjectData;
        
		#region Properties
		//[JsonIgnore] public bool ExampleCheckBool => ExampleFlagData.HasFlag(SaveDataFlags.ExampleOne);
		#endregion

		public void InitialiseSaveData()
		{
			InventoryData = new InventoryData();
			QuestSaveData = new QuestLogSaveData();
			ScriptableObjectData = new Dictionary<int, SOData>();
			
			InventoryData.Initialise();
			QuestSaveData.Initialise();
		}

		public void SetFlags(AnimalFlags flag, bool value)
		{
			if (value)
				AnimalData |= flag;
			else
				AnimalData &= ~flag;
		}
		
		public void SetFlags(FishFlags flag, bool value)
		{
			if (value)
				FishData |= flag;
			else
				FishData &= ~flag;
		}
		
		public void SetFlags(BirdFlags flag, bool value)
		{
			if (value)
				BirdData |= flag;
			else
				BirdData &= ~flag;
		}
	}
}