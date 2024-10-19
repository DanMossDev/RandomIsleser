using System.Threading.Tasks;
using UnityEngine;
using TMPro;

namespace RandomIsleser
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _characterName;
        [SerializeField] private TextMeshProUGUI _dialogue;

        [SerializeField, Tooltip("Amount of time in MS for each character to show")] private int _textAnimationRate = 10;

        private bool _skipText = false;
        public bool IsFinishedDisplaying { get; private set; }
        
        public DialogueNode CurrentNode { get; private set; }

        public void ShowDialogueNode(DialogueNode node)
        {
            CurrentNode = node;
            _skipText = false;
            IsFinishedDisplaying = false;
            var speaker = node.GetSpeaker();
            _characterName.text = speaker.Name.GetLocalizedString();
            
            Services.Instance.CameraManager.SetCurrentSpeaker(speaker.Controller);
            
            AnimateDialogue(node.GetDialogue());
        }

        public async void AnimateDialogue(string text)
        {
            string showText = "";

            while (showText.Length < text.Length && !_skipText)
            {
                bool foundRichText = text[showText.Length] == '<';

                while (foundRichText)
                {
                    showText += text[showText.Length];
                    if (showText[^1] == '>')
                        foundRichText = false;
                }
                if (showText.Length < text.Length)
                    showText += text[showText.Length];
                
                _dialogue.text = showText;
                await Task.Delay(_textAnimationRate, destroyCancellationToken);
            }
            IsFinishedDisplaying = true;
            _dialogue.text = text;
        }

        public void SkipDialogue()
        {
            _skipText = true;
        }
    }
}
