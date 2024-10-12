using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "CurrencyPickupModel", menuName = "RandomIsler/Models/Pickups/CurrencyModel")]
    public class CurrencyPickupModel : PickupModel
    {
        [SerializeField] private int _value;
        
        public override void PickUp()
        {
            Services.Instance.RuntimeSaveManager.LocalSaveData.InventoryData.AddCurrency(_value);
            Services.Instance.UIManager.UpdateCurrency(_value);
        }
    }
}
