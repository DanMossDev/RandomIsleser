using UnityEngine;

namespace RandomIsleser
{
    public class HammerController : EquippableController
    {
        [SerializeField] private HammerModel _model;
        public override int ItemIndex => _model.ItemIndex;
    }
}
