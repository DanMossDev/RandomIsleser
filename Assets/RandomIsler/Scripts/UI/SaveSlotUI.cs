using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace RandomIsleser
{
    public class SaveSlotUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _slotLabel;
        [SerializeField] private TextMeshProUGUI _playerName;
        [SerializeField] private TextMeshProUGUI _location;
        [SerializeField] private TextMeshProUGUI _timePlay;

        [SerializeField] private TextMeshProUGUI _newGameText;

        private bool _isNewSlot = true;
        private int _slot = -1;

        public void Populate(int slot, SaveSlotData data)
        {
            _slotLabel.text = data.DisplaySlot.ToString();
            _slot = slot;

            if (!data.DataExists)
            {
                _playerName.text = string.Empty;
                _location.text = string.Empty;
                _timePlay.text = string.Empty;
                _newGameText.gameObject.SetActive(true);
                _isNewSlot = true;
                return;
            }
            _isNewSlot = false;
            _newGameText.gameObject.SetActive(false);

            _playerName.text = data.PlayerName;
            _location.text = data.LastSceneName;
            _timePlay.text = data.TimePlayed.ToString(@"hh\:mm\:ss");
        }

        public void GoToFileManagement()
        {
            MenuScreenManager.Instance.GoToFileManage(_slot);
        }
    }
}
