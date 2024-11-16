using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RandomIsleser
{
    public class NameSelectScreen : MenuScreen
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        
        private int _slot;
        
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

        public void SetSlot(int slot)
        {
            _slot = slot;
        }

        public void LetterPressed(char letter)
        {
            if (letter == '-')
                Backspace();
            else if (letter == '=')
                SetName();
            else
                _nameText.text += letter;
        }

        private void Back()
        {
            if (_nameText.text.Length > 0)
                Backspace();
            else
                MenuScreenManager.Instance.GoToFileSelect();
        }

        private void Backspace()
        {
            _nameText.text = _nameText.text.Substring(0, _nameText.text.Length - 1);
        }

        public void SetName()
        {
            RuntimeSaveManager.Instance.LocalSaveData.SaveSlots[_slot].PlayerName = "Test";
            MenuScreenManager.Instance.GoToFileManage(_slot);
        }
    }
}
