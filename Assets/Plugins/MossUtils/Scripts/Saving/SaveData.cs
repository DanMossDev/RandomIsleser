using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MossUtils
{
	[Serializable]
	public class SaveData
	{
		public int ExampleData;
		public SaveDataFlags ExampleFlagData;
        
		#region Properties
		[JsonIgnore] public bool ExampleCheckBool => ExampleFlagData.HasFlag(SaveDataFlags.ExampleOne);
		#endregion

		public void InitialiseSaveData()
		{
			ExampleFlagData = SaveDataFlags.None;
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