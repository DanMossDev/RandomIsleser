using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class DialogueManager : MonoBehaviour
    {
        private Dictionary<int, DialogueNode> _dialogueNodes = new Dictionary<int, DialogueNode>();
        
        private void Awake()
        {
            foreach (var dialogueNode in SaveableObjectHelper.Instance.AllDialogueNodes)
                _dialogueNodes.Add(dialogueNode.ID, dialogueNode);
        }
        
        public void LoadDialogueData(List<DialogueData> dialogue)
        {
            foreach (var node in dialogue)
            {
                _dialogueNodes[node.ID].Load(node);
            }
        }
    }
}
