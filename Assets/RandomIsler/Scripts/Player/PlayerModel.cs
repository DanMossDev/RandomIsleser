using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "PlayerModel", menuName = AssetMenuNames.Models + "Player")]
    public class PlayerModel : ScriptableObject
    {
        [Header("Player Model")]
        [Space, Header("Movement")]
        [SerializeField] private float _movementSpeed = 10;
        [SerializeField] private float _rotationSpeed = 10;
        [SerializeField] private float _attackingRotationMultiplier = 0.2f;
        [SerializeField] private float _gravityMultiplier = 5;
        
        [Space, Header("Rolling")]
        [SerializeField] private float _rollSpeed = 10;
        [SerializeField] private float _rollDuration = 0.4f;

        [Space, Header("Swimming")]
        [SerializeField] private float _swimSpeed = 5;
        [SerializeField] private float _buoyancyForce = 3f;
        [SerializeField] private float _swimDrag = 3;
        
        [Space, Header("Ladder Climb")]
        [SerializeField] private float _ladderClimbDistance = 0.5f;
        
        [Space, Header("Aiming")]
        [SerializeField] private float _aimSpeed = 90;
        
        
        //Properties
        public float MovementSpeed => _movementSpeed;
        public float RollSpeed => _rollSpeed;
        public float RollDuration => _rollDuration;
        public float RotationSpeed => _rotationSpeed;
        public float AttackingRotationMultiplier  => _attackingRotationMultiplier;
        public float SwimSpeed => _swimSpeed;
        public float BuoyancyForce => _buoyancyForce;
        public float SwimDrag => _swimDrag;
        public float LadderClimbDistance => _ladderClimbDistance;
        public float AimSpeed => _aimSpeed; //TODO get this from settings instead
        public float GravityMultiplier => _gravityMultiplier;
    }
}
