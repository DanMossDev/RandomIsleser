using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RandomIsleser
{
    public class EquippableInventoryItemPanel : MonoBehaviour
    {
        public EquippableModel Item;

        private Image _sprite;
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _sprite = GetComponentInChildren<Image>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            if (Item.Unlocked)
            {
                _text.text = Item.Name.GetLocalizedString();
                _sprite.sprite = Item.Sprite;
            }
        }
    }
}
