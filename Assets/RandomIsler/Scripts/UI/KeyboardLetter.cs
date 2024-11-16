using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RandomIsleser
{
    public class KeyboardLetter : MonoBehaviour
    {
        [SerializeField] private char _letter;
        [SerializeField] private TextMeshProUGUI _text;

        [SerializeField] private InputAction _inputAction;
        
        private NameSelectScreen _nameSelectScreen;

        private void Awake()
        {
            _nameSelectScreen = GetComponentInParent<NameSelectScreen>();
        }

        private void OnEnable()
        {
            _text.text = _letter.ToString();
            
            _inputAction.Enable();
            _inputAction.performed += LetterPressed;
        }

        private void OnDisable()
        {
            _inputAction.performed -= LetterPressed;
        }

        private void LetterPressed(InputAction.CallbackContext context)
        {
            LetterPressed();
        }

        public void LetterPressed()
        {
            _nameSelectScreen.LetterPressed(_letter);
        }
    }
}
