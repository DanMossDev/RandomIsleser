using TMPro;
using UnityEngine;

namespace RandomIsleser
{
    public class FileManageScreen : MenuScreen
    {
        [SerializeField] private TextMeshProUGUI _playerName;
        [SerializeField] private TextMeshProUGUI _location;
        [SerializeField] private TextMeshProUGUI _timePlay;
        
        private SaveSlotData _saveData;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            InputManager.BackInput += Back;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            InputManager.BackInput -= Back;
        }
        
        public void Populate(SaveSlotData slotData)
        {
            _saveData = slotData;
            _playerName.text = slotData.PlayerName;
            _location.text = slotData.LastSceneName;
            _timePlay.text = slotData.TimePlayed.ToString(@"hh\:mm\:ss");
        }

        public void StartGame()
        {
            if (string.IsNullOrEmpty(_saveData.LastSceneName))
                _saveData.LastSceneName = "TestOpenWorld";
            
            RuntimeSaveManager.Instance.LoadIntoSlot(_saveData.Slot);
        }
        
        public void Back()
        {
            _manager.GoToFileSelect();
        }

        public void Delete()
        {
            //delete save;
            Back();
        }
    }
}
