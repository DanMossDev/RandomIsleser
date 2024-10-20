using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

namespace MossUtils
{
	public class SaveHandlerPersistent : ISaveHandler
	{
		public static readonly string k_SaveFilePath = $"{Application.persistentDataPath}/saves/save.json";
		public async Task<bool> SaveGame(SaveData dataToSave)
		{
			try
			{
				Directory.CreateDirectory(Path.GetDirectoryName(k_SaveFilePath));
				JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
				await File.WriteAllTextAsync(k_SaveFilePath, JsonConvert.SerializeObject(dataToSave, settings));
				return true;
			}
			catch (System.Exception error)
			{
				Debug.LogError($"Failed to write to {k_SaveFilePath} with exception {error}");
				return false;
			}
		}

		public async Task<SaveData> LoadSaveData()
		{
			string jsonData;
			try
			{
				jsonData = await File.ReadAllTextAsync(k_SaveFilePath);
			}
			catch (System.Exception error)
			{
				Debug.Log($"Failed to load from {k_SaveFilePath} with exception {error}");
				return null;
			}
			JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
			SaveData loadedSaveData = JsonConvert.DeserializeObject<SaveData>(jsonData, settings);
			return loadedSaveData;
		}

		public async Task<bool> DeleteSaveData()
		{
			await Task.Yield();
			try
			{
				File.Delete(k_SaveFilePath);
				return true;
			}
			catch (System.Exception error)
			{
				Debug.LogError($"Failed to delete {k_SaveFilePath} with exception {error}");
				return false;
			}
		}
	}
}