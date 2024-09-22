using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "PlayerModel", menuName = "RandomIsler/Models/Player")]
    public class PlayerModel : ScriptableObject
    {
        [SerializeField] private float _movementSpeed;
        
        public float MovementSpeed => _movementSpeed;
    }
}
