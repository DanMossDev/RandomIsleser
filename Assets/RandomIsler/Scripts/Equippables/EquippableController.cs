using UnityEngine;

namespace RandomIsleser
{
    public class EquippableController : MonoBehaviour
    {
        [SerializeField] private bool _isAimable = true;

        [SerializeField] private GameObject _leftHandIKTarget;
        [SerializeField] private GameObject _rightHandIKTarget;
        
        public virtual int ItemIndex => -1;
        public bool IsAimable => _isAimable;
        
        private void OnEnable()
        {
            Initialise();
        }
        
        private void OnDisable()
        {
            Cleanup();
        }
        
        protected virtual void Initialise()
        { }
        
        protected virtual void Cleanup()
        { }

        protected virtual void SetIKTargets()
        {
            if (_leftHandIKTarget != null)
                PlayerController.Instance.SetLeftHandIK(true, _leftHandIKTarget.transform);
            if (_rightHandIKTarget != null)
                PlayerController.Instance.SetRightHandIK(true, _rightHandIKTarget.transform);
        }

        protected virtual void UnsetIKTargets()
        {
            PlayerController.Instance.SetLeftHandIK(false);
            PlayerController.Instance.SetRightHandIK(false);
        }
        
        public virtual void UpdateEquippable()
        { }
        
        public virtual void CheckAim(Vector3 origin, Vector3 aimDirection)
        { }
        
        public virtual void UseItem()
        { }
        
        public virtual void ReleaseItem()
        { }

        public virtual void OnUnequip()
        {
            UnsetIKTargets();
        }

        public virtual void OnEquip()
        {
            SetIKTargets();
        }
    }
}
