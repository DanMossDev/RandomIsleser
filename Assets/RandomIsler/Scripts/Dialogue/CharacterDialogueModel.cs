using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "CharacterDialogueModel", menuName = AssetMenuNames.Dialogue + "CharacterDialogueModel")]
    public class CharacterDialogueModel : ScriptableObject
    {
        [SerializeField] private List<DialogueTree> _genericDialogueTrees;

        public DialogueTree GetRandomDialogueTree()
        {
            foreach (var tree in _genericDialogueTrees)
            {
                if (!tree.HasBeenSeen)
                    return tree;
            }
            
            return _genericDialogueTrees[Random.Range(0, _genericDialogueTrees.Count)];
        }
    }
}
