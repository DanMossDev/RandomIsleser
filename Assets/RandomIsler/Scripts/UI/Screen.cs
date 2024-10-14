using UnityEngine;
using UnityEngine.EventSystems;

namespace RandomIsleser
{
    public class Screen : MonoBehaviour
    {
        [SerializeField] private GameObject _firstSelected;

        private GameObject _lastSelected;

        protected virtual void Awake()
        {
            _lastSelected = _firstSelected;
        }

        protected virtual void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(_lastSelected);
        }
        
        protected virtual void OnDisable()
        {
            if (Application.isPlaying)
                _lastSelected = EventSystem.current.currentSelectedGameObject;
        }
    }
}
