using UnityEngine;

namespace RandomIsleser
{
    public class HammerController : EquippableController
    {
        [SerializeField] private HammerModel _model;
        public override int ItemIndex => _model.ItemIndex;

        private void OnTriggerEnter(Collider other)
        {
            if (!PlayerController.Instance.IsAttacking || !other.TryGetComponent(out Enemy enemy))
                return;
            
            enemy.TakeDamage(3);
        }
    }
}
