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
				return true;
			}

			return false;
		}

		public async void SaveGame()
		{
			if (_localSaveData != null)
			{
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