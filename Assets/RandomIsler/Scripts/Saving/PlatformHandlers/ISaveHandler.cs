using System.Threading.Tasks;

namespace MossUtils
{
	public interface ISaveHandler
	{
		public async Task<bool> SaveGame(SaveData dataToSave)
		{
			await Task.Yield();
			return false;
		}

		public async Task<SaveData> LoadSaveData()
		{
			await Task.Yield();
			return null;
		}

		public async Task<bool> DeleteSaveData()
		{
			await Task.Yield();
			return false;
		}
	}
}