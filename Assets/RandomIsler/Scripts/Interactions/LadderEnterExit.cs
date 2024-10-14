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
            _playerController.ExitLadder();
        }
    }
}
