using DG.Tweening;
using MossUtils;
using UnityEditor;
using UnityEngine;

namespace RandomIsleser
{
    public class MenuScreenManager : MonoSingleton<MenuScreenManager>
    {
        private MenuScreen _currentScreen;

        [SerializeField] private MenuScreen _engageScreen;
        [SerializeField] private MenuScreen _menuScreen;
        [SerializeField] private MenuScreen _fileSelectScreen;
        [SerializeField] private FileManageScreen _fileManageScreen;
        [SerializeField] private NameSelectScreen _nameSelectScreen;
        [SerializeField] private MenuScreen _settingsScreen;

        private void Start()
        {
            _currentScreen = _engageScreen;
            _currentScreen.CanvasGroup.DOFade(1, 1);
            _currentScreen.gameObject.SetActive(true);
        }

        public void GoToEngageScreen()
        {
            SetScreen(_engageScreen);
        }

        public void GoToMainMenu()
        {
            SetScreen(_menuScreen);
        }

        public void GoToFileSelect()
        {
            SetScreen(_fileSelectScreen);
        }

        public void GoToFileManage(int slot)
        {
            SaveSlotData slotData = RuntimeSaveManager.Instance.LocalSaveData.SaveSlots[slot];

            if (slotData.DataExists)
            {
                _fileManageScreen.Populate(slotData);
                SetScreen(_fileManageScreen);
                return;
            }

            _nameSelectScreen.SetSlot(slot);
            SetScreen(_nameSelectScreen);
        }

        public void StartGame(int slot)
        {
            RuntimeSaveManager.Instance.LoadIntoSlot(slot);
        }

        public void GoToSettingsScreen()
        {
            SetScreen(_settingsScreen);
        }

        private void SetScreen(MenuScreen screen)
        {
            _currentScreen.CanvasGroup.DOFade(0,1).OnComplete(()=>
            {
                _currentScreen.gameObject.SetActive(false);
                _currentScreen = screen;
                _currentScreen.CanvasGroup.DOFade(1,1).OnStart(()=>_currentScreen.gameObject.SetActive(true));
            });
        }

        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            return;
#endif
            Application.Quit();
        }
    }
}
