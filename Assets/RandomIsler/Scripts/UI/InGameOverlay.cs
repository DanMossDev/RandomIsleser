using TMPro;
using UnityEngine;

namespace RandomIsleser
{
    public class InGameOverlay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currencyOverlayText;
        [SerializeField] private int _timeToDisplayOne;

        private int _currentAmount;
        private int _amountToAdd;
        

        private void UpdateText()
        {
            _currencyOverlayText.text = _currentAmount.ToString("D3");
        }

        public void InstantlySetCurrency(int amount)
        {
            _currentAmount = amount;
            UpdateText();
        }

        public void UpdateCurrency(int change)
        {
            _amountToAdd += change;
        }

        private void FixedUpdate()
        {
            if (_amountToAdd == 0)
                return;

            if (_amountToAdd > 0)
            {
                _currentAmount++;
                _amountToAdd--;
            }
            else
            {
                _currentAmount--;
                _amountToAdd++;
            }
            UpdateText();
        }
    }
}
