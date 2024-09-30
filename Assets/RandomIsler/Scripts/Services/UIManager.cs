using UnityEngine;

namespace RandomIsleser
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _aimingUI;
        [SerializeField] private GameObject _aimingReticle;
        [SerializeField] private GameObject _interactReticle;

        public void SetAimingUIVisible(bool show)
        {
            _aimingUI.SetActive(show);
        }

        public void SetAimingReticle(bool isInteractable)
        {
            _aimingReticle.SetActive(!isInteractable);
            _interactReticle.SetActive(isInteractable);
        }
    }
}
