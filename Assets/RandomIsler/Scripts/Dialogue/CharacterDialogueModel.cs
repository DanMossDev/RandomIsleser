using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "CharacterDialogueModel", menuName = "RandomIsler/Models/CharacterDialogueModel")]
    public class CharacterDialogueModel : ScriptableObject
    {
        [SerializeField] private List<DialogueNode> _dialogueNodes;

        public DialogueNode GetRandomDialogueNode()
        {
            foreach (var node in _dialogueNodes)
            {
                if (!node.HasBeenSeen)
                    return node;
            }
            
            return _dialogueNodes[Random.Range(0, _dialogueNodes.Count)];
        }
    }
}
