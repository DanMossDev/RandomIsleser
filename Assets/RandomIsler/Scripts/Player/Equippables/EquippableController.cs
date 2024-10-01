using UnityEngine;

namespace RandomIsleser
{
    public class EquippableController : MonoBehaviour
    {
        [SerializeField] private bool _isAimable = true;
        
        public virtual int ItemIndex => -1;
        public bool IsAimable => _isAimable;
        
        public virtual void CheckAim(Vector3 aimDirection)
        { }
        
        public virtual void UseItem()
        { }
    }
}
