using System;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "DialogueTree", menuName = AssetMenuNames.Dialogue + "DialogueTree")]
    public class DialogueTree : SaveableObject
    {
        [SerializeField] private DialogueNode _firstDialogueNode;
        
        public UnlockCriteria UnlockCriteria;
        public bool HasBeenSeen = false;

        [HideInInspector] public List<NPCModel> Participants;
        public bool CanDialogueBePlayed => UnlockCriteria.IsUnlocked();

        public DialogueNode GetFirstDialogueNode()
        {
            HasBeenSeen = true;
            Services.Instance.RuntimeSaveManager.LocalSaveData.ScriptableObjectData[ID] = GetData();
            return _firstDialogueNode;
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            
            HasBeenSeen = false;

            _firstDialogueNode.Owner = this;
        }

        public override void Load(SOData data)
        {
            var dialogueData = data as DialogueData;
            if (dialogueData == null)
                return;
            
            HasBeenSeen = dialogueData.HasBeenSeen;
        }

        public override SOData GetData()
        {
            return new DialogueData()
            {
                ID = ID,
                Name = name,
                HasBeenSeen = HasBeenSeen
            };
        }
    }
    
    [Serializable]
    public class DialogueData : SOData
    {
	    public bool HasBeenSeen = false;
    }
}
