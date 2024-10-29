using UnityEngine;

namespace RandomIsleser
{
    public class Mirror : SolarChargeAbsorber
    {
        [SerializeField] private SolarPanelModel _model;
        
        private Collider[] _beamHitCheck = new Collider[3];
        
        public override void ReceiveCharge(float amount)
        { }

        public override void Reflect(Vector3 point, Vector3 normal, LineRenderer lineRenderer)
        {
            ReflectLightBeam(point, normal, lineRenderer);
        }
        
        private void ReflectLightBeam(Vector3 origin, Vector3 dir, LineRenderer lineRenderer)
        {
            lineRenderer.positionCount++;

            if (Physics.SphereCast(origin, _model.RayWidth, dir, out RaycastHit info, _model.MaxDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, info.point);

                var amount = Physics.OverlapSphereNonAlloc(info.point, 1, _beamHitCheck, _model.InteractionLayers, QueryTriggerInteraction.Collide);
                for (int i = 0; i < amount; i++)
                {
                    if (_beamHitCheck[i].TryGetComponent(out SolarChargeAbsorber absorber))
                    {
                        absorber.ReceiveCharge(Time.deltaTime);
                        absorber.Reflect(info.point, info.normal, lineRenderer);
                    }
                }
            }
            else
                lineRenderer.SetPosition(lineRenderer.positionCount - 1,origin + dir * _model.MaxDistance);
        }
    }
}
