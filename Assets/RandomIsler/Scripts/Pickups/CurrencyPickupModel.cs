using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "CurrencyPickupModel", menuName = "RandomIsler/Models/Pickups/CurrencyModel")]
    public class CurrencyPickupModel : PickupModel
    {
        [SerializeField] private int _value;
        
        public override void PickUp()
        {
            PlayerController.Instance.AddCurrency(_value);
        }
    }
}
