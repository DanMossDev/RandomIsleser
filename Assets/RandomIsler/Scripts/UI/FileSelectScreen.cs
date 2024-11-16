using UnityEngine;

namespace RandomIsleser
{
    public class FileSelectScreen : MenuScreen
    {
        [SerializeField] private SaveSlotUI[] _saveSlots;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            InputManager.BackInput += Back;
            
            for (int i = 0; i < _saveSlots.Length; i++)
                _saveSlots[i].Populate(i, RuntimeSaveManager.Instance.LocalSaveData.SaveSlots[i]);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            InputManager.BackInput -= Back;
        }
        
        private void Back()
        {
            _manager.GoToMainMenu();
        }
    }
}
