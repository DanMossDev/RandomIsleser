using System;
using RandomIsleser;

namespace MossUtils
{
	[Serializable]
	public class SaveData
	{
		public AnimalFlags AnimalData;
		public AnimalFlags RareAnimalData;

		public SaveSlotData[] SaveSlots;
		
        
		#region Properties
		//[JsonIgnore] public bool ExampleCheckBool => ExampleFlagData.HasFlag(SaveDataFlags.ExampleOne);
		#endregion

		public void InitialiseSaveData()
		{
			SaveSlots = new SaveSlotData[3];

			for (int i = 0; i < SaveSlots.Length; i++)
			{
				SaveSlots[i] = new SaveSlotData();
				SaveSlots[i].Initialise(i);
			}
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