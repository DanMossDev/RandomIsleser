using System.Collections.Generic;
using System.Threading.Tasks;
using MossUtils;
using UnityEngine;

namespace RandomIsleser
{
	public class RuntimeSaveManager : MonoSingleton<RuntimeSaveManager>
	{

		private SaveData _localSaveData = null;
		public SaveData LocalSaveData => _localSaveData;

		private int _loadedSlot = 0;
		public SaveSlotData CurrentSaveSlot => _localSaveData.SaveSlots[_loadedSlot];

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
				LoadSaveableObjectData(CurrentSaveSlot.ScriptableObjectData);
				return true;
			}

			return false;
		}

		public void BeginNewGame(int selectedSlot, string playerName) //consider removing this
		{
			if (selectedSlot == _loadedSlot)
			{
				_localSaveData.SaveSlots[selectedSlot] = new SaveSlotData();
				_loadedSlot = selectedSlot;
			}
			
			CurrentSaveSlot.PlayerName = playerName;
			SceneTransitionManager.LoadOpeningScene();
		}

		public void LoadIntoSlot(int selectedSlot)
		{
			_loadedSlot = selectedSlot;
			LoadSaveableObjectData(CurrentSaveSlot.ScriptableObjectData);

			CurrentSaveSlot.Loaded();
			LoadSavedScene();
		}

		private void LoadSavedScene()
		{
			SceneTransitionManager.LoadScene(CurrentSaveSlot.LastSceneName);
		}
		
		private void LoadSaveableObjectData(Dictionary<int, SOData> data)
		{
			CurrentSaveSlot.ResetRuntimeValues();
			
			var saveableObjects = SaveableObjectHelper.Instance.AllSaveableObjectsDictionary;
			foreach (var (id, datum) in data) 
				saveableObjects[id].Load(datum);
		}

		public async void SaveGame()
		{
			if (_localSaveData != null)
			{
				if (SceneTransitionManager.CurrentSceneIsSaveable())
				{
					CurrentSaveSlot.LastSceneName = SceneTransitionManager.CurrentScene;
					CurrentSaveSlot.Saved(); 
					//Serialize something to the effect of "spawn location"
				}
				
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