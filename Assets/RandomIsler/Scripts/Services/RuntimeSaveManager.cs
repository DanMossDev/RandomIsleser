using System.Collections.Generic;
using System.Threading.Tasks;
using MossUtils;
using UnityEngine;

namespace RandomIsleser
{
	public class RuntimeSaveManager : MonoBehaviour
	{

		private SaveData _localSaveData = null;
		public SaveData LocalSaveData => _localSaveData;

		public bool ReadyToLoad => _localSaveData != null;
		public bool IsFTUE { get; private set; } = false;

		public async void Awake()
		{
			if (!await LoadGame())
				await CreateGame();
		}

		public async void DeleteGame()
		{
			await SaveDataManager.DeleteSaveData();
		}

		public async Task CreateGame()
		{
			IsFTUE = true;
			SaveData newSaveData = await SaveDataManager.CreateSaveData();

			if (newSaveData != null)
			{
				_localSaveData = newSaveData;
			}
		}

		public async Task<bool> LoadGame()
		{
			SaveData loadedSaveData = await SaveDataManager.LoadSaveData();

			if (loadedSaveData != null)
			{
				_localSaveData = loadedSaveData;
				Services.Instance.UIManager.InstantlySetCurrency(_localSaveData.InventoryData.Currency);
				
				//Load Scriptable Objects
				LoadSaveableObjectData(_localSaveData.ScriptableObjectData);
				
				return true;
			}

			return false;
		}
		
		private void LoadSaveableObjectData(Dictionary<int, SOData> data)
		{
			var saveableObjects = SaveableObjectHelper.Instance.AllSaveableObjectsDictionary;
			foreach (var (id, datum) in data) 
				saveableObjects[id].Load(datum);
		}

		public async void SaveGame()
		{
			if (_localSaveData != null)
			{
				SaveableObjectHelper.Instance.SaveScriptableData();
				await SaveDataManager.SaveGame(_localSaveData);
			}
		}

		public void ExitApp()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
		}
	}
}