using UnityEngine;

namespace RandomIsleser
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _aimingUI;
        [SerializeField] private GameObject _aimingReticle;
        [SerializeField] private GameObject _interactReticle;
        [SerializeField] private GameObject _pauseUI;

        [SerializeField] private InGameOverlay _inGameOverlay;
        [SerializeField] private DialogueUI _dialogueUI;
        
        [SerializeField] private ItemGetUI _itemGetUI;
        
        public DialogueUI DialogueUI => _dialogueUI;

        private async void Start()
        {
            while (!RuntimeSaveManager.Instance || !RuntimeSaveManager.Instance.ReadyToLoad)
                await System.Threading.Tasks.Task.Delay(100);
            InstantlySetCurrency(RuntimeSaveManager.Instance.CurrentSaveSlot.InventoryData.Currency);
        }
        
        #region Overlay
        public void SetPause(bool show)
        {
            _pauseUI.SetActive(show);
            _inGameOverlay.gameObject.SetActive(!show);
        }

        public void InstantlySetCurrency(int amount)
        {
            _inGameOverlay.InstantlySetCurrency(amount);
        }

        public void UpdateCurrency(int change)
        {
            _inGameOverlay.UpdateCurrency(change);
        }
        #endregion

        #region Aiming
        public void SetAimingUIVisible(bool show)
        {
            _aimingUI.SetActive(show);
            _inGameOverlay.gameObject.SetActive(!show);
        }

        public void SetAimingReticle(bool isInteractable)
        {
            _aimingReticle.SetActive(!isInteractable);
            _interactReticle.SetActive(isInteractable);
        }
        #endregion
        
        #region Dialogue
        public void BeginDialogue(DialogueNode dialogueNode)
        {
            _dialogueUI.gameObject.SetActive(true);
            _dialogueUI.ShowDialogueNode(dialogueNode);
        }

        public void SkipDialogue()
        {
            _dialogueUI.SkipDialogue();
        }

        public void ContinueDialogue()
        {
            _dialogueUI.ShowDialogueNode(_dialogueUI.CurrentNode.GetNextDialogue());
        }

        public void EndDialogue()
        {
            _dialogueUI.gameObject.SetActive(false);
        }
        #endregion

        #region Context Sensitive UI

        public void ShowItemGetUI(PickupModel pickup)
        {
            _itemGetUI.Populate(pickup);
            _itemGetUI.gameObject.SetActive(true);
        }

        public bool CloseItemGetUI()
        {
            if (_itemGetUI.CanClose)
            {
                _itemGetUI.gameObject.SetActive(false);
                return true;
            }

            return false;
        }

        #endregion
    }
}
