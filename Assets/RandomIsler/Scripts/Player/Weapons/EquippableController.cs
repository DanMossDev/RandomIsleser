using UnityEngine;

namespace RandomIsleser
{
    public class EquippableController : MonoBehaviour
    {
        [SerializeField] private int _itemIndex;
        [SerializeField] private bool _isAimable = true;
        
        public int ItemIndex => _itemIndex;
        public bool IsAimable => _isAimable;
        
        public virtual void CheckAim(Vector3 aimDirection)
        { }
        
        public virtual void UseItem()
        { }
    }
}
