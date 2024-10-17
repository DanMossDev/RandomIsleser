using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class DialogueManager : MonoBehaviour
    {
        private Dictionary<int, DialogueTree> _dialogueTrees = new Dictionary<int, DialogueTree>();
        
        private void Awake()
        {
            foreach (var dialogueTree in SaveableObjectHelper.Instance.AllDialogueTrees)
                _dialogueTrees.Add(dialogueTree.ID, dialogueTree);
        }
        
        public void LoadDialogueData(List<DialogueData> dialogue)
        {
            foreach (var tree in dialogue)
            {
                _dialogueTrees[tree.ID].Load(tree);
            }
        }

        public void BeginDialogueTree(DialogueTree dialogueTree)
        {
            //probably should stop player controls
            //Listen for dialogue controls
            //move to the dialogue camera
            //show dialogue UI
            //play first dialogue instance
            
            Debug.Log(dialogueTree.GetFirstDialogueNode().GetDialogue());
        }
    }
}
