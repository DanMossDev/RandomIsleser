using UnityEngine;

namespace RandomIsleser
{
    public class EquippableController : MonoBehaviour
    {
        [SerializeField] private bool _isAimable = true;
        
        public virtual int ItemIndex => -1;
        public bool IsAimable => _isAimable;
        
        private void OnEnable()
        {
            Initialise();
        }
        
        protected virtual void Initialise()
        { }
        
        public virtual void UpdateEquippable()
        { }
        
        public virtual void CheckAim(Vector3 aimDirection)
        { }
        
        public virtual void UseItem()
        { }
        
        public virtual void ReleaseItem()
        { }

        public virtual void OnUnequip()
        { }
        
        public virtual void OnEquip()
        { }
    }
}
