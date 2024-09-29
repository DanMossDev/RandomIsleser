using UnityEngine;

namespace RandomIsleser
{
    public class FishingRodController : AimableController
    {
        [SerializeField] private AimableModel _model;
        
        public override void CheckAim(Vector3 aimDirection)
        {
            Debug.DrawLine(transform.position, transform.position + (aimDirection * _model.Range), Color.red);
            if (Physics.SphereCast(
                    transform.position, 
                    _model.AimTolerance, 
                    aimDirection, 
                    out RaycastHit hit,
                    _model.Range, 
                    _model.HitLayers))
            {
                if ((_model.InteractLayer & 1 << hit.collider.gameObject.layer) != 0)
                {
                    Debug.Log("You can interact with this!");
                }
            }
        }

        public override void Shoot(Vector3 aimDirection)
        {
            
        }
    }
}
