using UnityEngine;
using UnityEngine.Localization;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "DialogueNode", menuName = "RandomIsler/Dialogue/DialogueNode")]
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private NPCModel _npcModel;
        [SerializeField] private LocalizedString _dialogue;
        [SerializeField] private DialogueNode _nextDialogue;

        public bool IsEnd => _nextDialogue == null;

        public string GetSpeaker()
        {
            return _npcModel.Name.GetLocalizedString();
        }
        
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
