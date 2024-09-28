using UnityEngine;

namespace RandomIsleser
{
    [RequireComponent(typeof(Rigidbody))]
    public class BuoyancyController : MonoBehaviour
    {
        [SerializeField] private Transform[] _buoyancyPoints;
        
        [SerializeField] private float _waterHeight = 0f;
        
        [SerializeField] private float _underwaterDrag = 3;
        [SerializeField] private float _underwaterAngularDrag = 1;

        [SerializeField] private float _floatingPower = 15f;
        
        private Rigidbody _rigidbody;

        private bool _underWater;
        
        private int _buoyancyPointsUnderWater;
        
        private float _defaultDrag = 0;
        private float _defaultAngularDrag = 0.05f;

        private void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _defaultDrag = _rigidbody.drag;
            _defaultAngularDrag = _rigidbody.angularDrag;
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _buoyancyPoints.Length; i++)
            {
                float diff = _buoyancyPoints[i].position.y - OceanController.Instance.GetHeightAtPosition(_buoyancyPoints[i].position);

                if (diff < 0)
                {
                    _rigidbody.AddForceAtPosition(_floatingPower * Mathf.Abs(diff) * Vector3.up, _buoyancyPoints[i].position, ForceMode.Force);
                    _buoyancyPointsUnderWater++;
                    if (!_underWater)
                    {
                        _underWater = true;
                        SwitchDragVariables(_underWater);
                    }
                }
            }
            
            if (_underWater && _buoyancyPointsUnderWater == 0)
            {
                _underWater = false;
                SwitchDragVariables(_underWater);
            }
            _buoyancyPointsUnderWater = 0;
        }

        private void SwitchDragVariables(bool isUnderwater)
        {
            _rigidbody.drag = isUnderwater ? _underwaterDrag : _defaultDrag;
            _rigidbody.angularDrag = isUnderwater ? _underwaterAngularDrag : _defaultAngularDrag;
        }
    }
}
