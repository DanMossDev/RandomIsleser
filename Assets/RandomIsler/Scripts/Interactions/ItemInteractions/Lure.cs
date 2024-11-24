using UnityEngine;

namespace RandomIsleser
{
    public class Lure : MonoBehaviour
    {
        [SerializeField] private float _nibbleForce = 1;
        [SerializeField] private float _biteForce = 5;

        [SerializeField] private float _interactCooldown = 3;
        
        [SerializeField] private float _reelFloatingPower = 10;

        public float LureStrength = 1;

        private Rigidbody _rigidbody;

        private float _interactTime;
        private float _timeBit;
        
        private float _fishBiteTolerance;
        private float _initialFloatingPower;
        
        private bool _biting;

        public int Followers { get; private set; } = 1;

        private FishController _fishController;
        private BuoyancyController _buoyancyController;
        private FishingRodController _fishingRodController;

        public FishingRodController FishingRodController => _fishingRodController;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _buoyancyController = GetComponent<BuoyancyController>();
            _initialFloatingPower = _buoyancyController.GetFloatingPower();
        }

        public void Init(FishingRodController fishingRodController)
        {
            _fishingRodController = fishingRodController;
        }

        public void BeginReel()
        {
            _fishController.transform.parent = transform;
            _fishController.BeginReel();
            
            if ((int)_buoyancyController.GetFloatingPower() != (int)_initialFloatingPower)
                _buoyancyController.SetFloatingPower(_reelFloatingPower);
        }

        public void ReelReturned()
        {
            if (_fishController != null)
                _fishController.Capture();
        }

        public void FishEscaped()
        {
            OnCast();
        }

        public void AddFollower()
        {
            Followers++;
        }

        public void RemoveFollower()
        {
            Followers--;
            if (Followers == 0)
                Followers = 1;
        }

        public void OnCast()
        {
            _fishController = null;
            _fishBiteTolerance = 1;
            _timeBit = 0;
            _interactTime = 0;
            _biting = false;
            _buoyancyController.SetFloatingPower(_initialFloatingPower);
        }

        public bool HasBite()
        {
            return _biting && Time.time - _timeBit < _fishBiteTolerance;
        }

        public void ApplyForce(Vector3 force)
        {
            _rigidbody.AddForce(force, ForceMode.Acceleration);
        }

        public bool Interact(FishController fishController)
        {
            if (_biting || Time.time - _interactTime < _interactCooldown)
                return false;
            
            _interactTime = Time.time;
            float roll = Random.Range(0f, 1f);

            if (roll > 0.75f)
            {
                Bite(fishController);
                return true;
            }
            
            Nibble();
            return false;
        }
        
        private void Nibble()
        {
            ApplyDownwardsForce(_nibbleForce);
        }

        private void Bite(FishController fishController)
        {
            _fishController = fishController;
            _fishBiteTolerance = fishController.BiteTolerance;
            _timeBit = Time.time;
            _biting = true;
            ApplyDownwardsForce(_biteForce);
            _buoyancyController.SetFloatingPower(0);
        }

        private void ApplyDownwardsForce(float force)
        {
            _rigidbody.AddForce(Vector3.up * -force, ForceMode.Impulse);
        }
    }
}
