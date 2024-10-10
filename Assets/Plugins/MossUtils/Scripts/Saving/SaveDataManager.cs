using System.Threading.Tasks;

namespace MossUtils
{
	public static class SaveDataManager
	{
		private static ISaveHandler _platformSaveHandler = new SaveHandlerPersistent();

		public static async Task<bool> SaveGame(SaveData dataToSave)
		{
			return await _platformSaveHandler.SaveGame(dataToSave);
		}

		public static async Task<SaveData> LoadSaveData()
		{
			return await _platformSaveHandler.LoadSaveData();
		}

		public static async Task<SaveData> CreateSaveData()
		{
			SaveData newSaveData = new SaveData();
			newSaveData.InitialiseSaveData();
			await SaveGame(newSaveData);
			return newSaveData;
		}

		public static async Task<bool> DeleteSaveData()
		{
			return await _platformSaveHandler.DeleteSaveData();
		}
	}
}