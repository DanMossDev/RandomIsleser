using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

namespace MossUtils
{
	public class SaveHandlerPersistent : ISaveHandler
	{
		private static readonly string kSaveFilePath = $"{Application.persistentDataPath}/saves/save.ppg";
		public async Task<bool> SaveGame(SaveData dataToSave)
		{
			try
			{
				Directory.CreateDirectory(Path.GetDirectoryName(kSaveFilePath));
				await File.WriteAllTextAsync(kSaveFilePath, JsonConvert.SerializeObject(dataToSave));
				return true;
			}
			catch (System.Exception error)
			{
				Debug.LogError($"Failed to write to {kSaveFilePath} with exception {error}");
				return false;
			}
		}

		public async Task<SaveData> LoadSaveData()
		{
			string jsonData;
			try
			{
				jsonData = await File.ReadAllTextAsync(kSaveFilePath);
			}
			catch (System.Exception error)
			{
				Debug.Log($"Failed to load from {kSaveFilePath} with exception {error}");
				return null;
			}

			SaveData loadedSaveData = JsonConvert.DeserializeObject<SaveData>(jsonData);
			return loadedSaveData;
		}

		public async Task<bool> DeleteSaveData()
		{
			await Task.Yield();
			try
			{
				File.Delete(kSaveFilePath);
				return true;
			}
			catch (System.Exception error)
			{
				Debug.LogError($"Failed to delete {kSaveFilePath} with exception {error}");
				return false;
			}
		}
	}
}