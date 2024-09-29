using UnityEngine;

namespace RandomIsleser
{
    public class AimableController : MonoBehaviour
    {
        public virtual void CheckAim(Vector3 aimDirection)
        { }
        
        public virtual void Shoot(Vector3 aimDirection)
        { }
    }
}
