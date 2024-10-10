using System;
using Newtonsoft.Json;

namespace MossUtils
{
	[Serializable]
	public class SaveData
	{
		public InventoryData InventoryData;
		public SaveDataFlags ExampleFlagData;
        
		#region Properties
		[JsonIgnore] public bool ExampleCheckBool => ExampleFlagData.HasFlag(SaveDataFlags.ExampleOne);
		#endregion

		public void InitialiseSaveData()
		{
			ExampleFlagData = SaveDataFlags.None;
			InventoryData = new InventoryData();
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