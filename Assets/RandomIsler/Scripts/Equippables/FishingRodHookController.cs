using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class FishingRodHookController : MonoBehaviour
    {
        private FishingRodController _owner;
        private Rigidbody _rigidbody;
        
        private bool _isCast = false;
        private bool _isReturn = false;

        private float _lerpRatio = 0;
        private float _speedMultiplier = 1;

        private Transform _parent;

        private Vector3 _startPosition;
        private Vector3 _targetPosition;

        private System.Action _onReturnCallback;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Init(FishingRodController owner)
        {
            _owner = owner;
        }

        public void CastRod(Vector3 targetPosition, float speedMulti)
        {
            _startPosition = transform.position;
            _targetPosition = targetPosition;
            _lerpRatio = 0;
            _speedMultiplier = speedMulti;
            
            transform.parent = null;
            
            EnableRigidbody(false);
            _isCast = true;
        }

        public void ReturnHook(Transform parent, float speedMulti = 1, System.Action callback = null)
        {
            _parent = parent;
            _startPosition = transform.position;
            _lerpRatio = 0;
            _onReturnCallback = callback;
            _speedMultiplier = speedMulti;
            
            _rigidbody.isKinematic = true;
            _isReturn = true;
            _isCast = false;
        }

        private void FixedUpdate()
        {
            if (!(_isCast || _isReturn)) return;

            _lerpRatio += Time.deltaTime * _speedMultiplier;

            if (_isCast)
            {
                transform.position = Vector3.Lerp(_startPosition, _targetPosition, _lerpRatio);
                
                if (_lerpRatio >= 1)
                {
                    _isCast = false;
                    _owner.ReelComplete();
                    return;
                }
                
                CheckForWater();
            }
            else
            {
                transform.localPosition = Vector3.Lerp(_startPosition, _parent.position, _lerpRatio);
                
                if (_lerpRatio >= 1)
                {
                    transform.parent = _parent;
                    transform.localPosition = Vector3.zero;
                    _isReturn = false;
                    _onReturnCallback?.Invoke();
                    _onReturnCallback = null;
                }
            }

        }

        private void EnableRigidbody(bool enable)
        {
            _rigidbody.isKinematic = !enable;
            _rigidbody.useGravity = enable;
        }
        
        private void CheckForWater()
        {
            if (OceanController.Instance.CheckIfUnderWater(transform.position))
            {
                Debug.Log("Hit water");
                BeginFishing();
            }
        }
        
        private void BeginFishing()
        {
            _isCast = false;
            EnableRigidbody(true);
            PlayerController.Instance.SetState(PlayerStates.FishingState);
        }
    }
}
