using UnityEngine;
using UnityEngine.Localization;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "DialogueNode", menuName = AssetMenuNames.Dialogue + "DialogueNode")]
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private NPCModel _npcModel;
        [SerializeField] private LocalizedString _dialogue;
        [SerializeField] private DialogueNode _nextDialogue;

        public bool IsEnd => _nextDialogue == null;

        [HideInInspector] public DialogueTree Owner;

        public NPCModel GetSpeaker()
        {
            return _npcModel;
        }
        
        public string GetDialogue()
        {
            return _dialogue.GetLocalizedString();
        }

        public DialogueNode GetNextDialogue()
        {
            return _nextDialogue;
        }

        private void OnValidate()
        {
            if (!IsEnd)
                _nextDialogue.Owner = Owner;
            
            if (Owner != null)
                if (!Owner.Participants.Contains(_npcModel))
                    Owner.Participants.Add(_npcModel);
        }
    }
}
