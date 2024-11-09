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
				case 1: return UnlockRareAnimal(flag);
				default: return UnlockNormalAnimal(flag);
			}
		}

		private bool UnlockNormalAnimal(AnimalFlags flag)
		{
			if (AnimalData.HasFlag(flag))
				return false;
			
			AnimalData |= flag;

			return true;
		}
		
		private bool UnlockRareAnimal(AnimalFlags flag)
		{
			if (RareAnimalData.HasFlag(flag))
				return false;
			
			RareAnimalData |= flag;

			return true;
		}
	}
}