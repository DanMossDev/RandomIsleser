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
            _characterName.text = node.GetSpeaker();
            
            AnimateDialogue(node.GetDialogue());
        }

        public async void AnimateDialogue(string text)
        {
            string showText = "";

            while (showText.Length < text.Length && !_skipText)
            {
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
