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
		public AnimalFlags RareAnimalData;
		
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

		public bool UnlockAnimal(AnimalFlags flag, int rarityVariant = 0)
		{
			switch (rarityVariant)
			{
				case 1: return UnlockAnimal(flag, ref RareAnimalData);
				default: return UnlockAnimal(flag, ref AnimalData);
			}
		}

		private bool UnlockAnimal(AnimalFlags flagToSet, ref AnimalFlags data)
		{
			if (data.HasFlag(flagToSet))
				return false;
			
			data |= flagToSet;

			return true;
		}
	}
}