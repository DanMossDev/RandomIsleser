using UnityEngine;
using UnityEngine.Localization;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "DialogueNode", menuName = "RandomIsler/Dialogue/DialogueNode")]
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private LocalizedString _dialogue;
        [SerializeField] private DialogueNode _nextDialogue;

        public bool IsEnd => _nextDialogue == null;

        public string GetDialogue()
        {
            return _dialogue.GetLocalizedString();
        }

        public DialogueNode GetNextDialogue()
        {
            return _nextDialogue;
        }
    }
}
