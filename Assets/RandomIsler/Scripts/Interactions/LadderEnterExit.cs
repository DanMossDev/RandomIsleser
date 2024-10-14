using UnityEngine;

namespace RandomIsleser
{
    public class LadderEnterExit : MonoBehaviour
    {
        [SerializeField] private bool _isTop;
        
        private PlayerController _playerController;

        private void Start()
        {
            _playerController = PlayerController.Instance;
        }
        
        private void OnTriggerStay(Collider other)
        {
            float lastInputY = PlayerController.Instance.LastInputDirection.z;
            bool shouldExit = (!_isTop && lastInputY < 0) || (_isTop && lastInputY > 0);
            if (shouldExit)
                _playerController.ExitLadder();
        }
    }
}
