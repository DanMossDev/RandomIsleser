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
            
            var lastMoveDir = _playerController.LastMoveDirection;
            if (lastMoveDir.z == 0)
                return;
            
            if (_isTop && lastMoveDir.z > 0)
                _playerController.ExitLadder();
            else if (!_isTop && lastMoveDir.z < 0)
                _playerController.ExitLadder();
        }
    }
}
