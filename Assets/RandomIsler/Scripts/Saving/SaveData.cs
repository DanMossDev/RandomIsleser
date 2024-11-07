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

		public bool UnlockAnimal(AnimalFlags flag)
		{
			if (AnimalData.HasFlag(flag))
				return false;
			
			AnimalData |= flag;

			return true;
		}
	}
}