using UnityEngine;

namespace RandomIsleser
{
    public class FishingLineController : MonoBehaviour
    {
        [SerializeField] private int _linePointsAmount = 10;

        private Transform[] _linePoints;
        private Transform _target;
        
        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _lineRenderer = gameObject.GetComponent<LineRenderer>();
            _lineRenderer.positionCount = _linePointsAmount;
            _linePoints = new Transform[_linePointsAmount];
            for (int i = 0; i < _linePointsAmount; i++)
            {
                var go = new GameObject($"LinePoint_{i}");
                go.transform.parent = transform;
                go.transform.localPosition = Vector3.zero;
                _linePoints[i] = go.transform;
            }
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void FixedUpdate()
        {
            _linePoints[0].localPosition = Vector3.zero;
            _linePoints[^1].position = _target.position;
            for (int i = 0; i < _linePointsAmount; i++)
            {
                _linePoints[i].position = Vector3.Lerp(_linePoints[0].position, _linePoints[^1].position, i / (_linePointsAmount - 1f));
                _lineRenderer.SetPosition(i, _linePoints[i].position);
            }
        }
    }
}
