using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "PlayerModel", menuName = "RandomIsler/Models/Player")]
    public class PlayerModel : ScriptableObject
    {
        [SerializeField] private float _movementSpeed = 10;
        [SerializeField] private float _rollSpeed = 10;
        [SerializeField] private float _rollDuration = 0.4f;
        
        public float MovementSpeed => _movementSpeed;
        public float RollSpeed => _rollSpeed;
        public float RollDuration => _rollDuration;
    }
}
