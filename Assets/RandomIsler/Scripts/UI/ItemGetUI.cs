using TMPro;
using UnityEngine;

namespace RandomIsleser
{
    public class ItemGetUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;

        [SerializeField] private float _minShowTime = 0.5f;

        private float _itemGetShowTime;

        public bool CanClose => Time.time - _itemGetShowTime > _minShowTime;
        
        public void Populate(PickupModel pickup)
        {
            _name.text = pickup.Name.GetLocalizedString();
            _description.text = pickup.FlavourText.GetLocalizedString();
            _itemGetShowTime = Time.time;
        }
    }
}
