using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "DialogueTree", menuName = "RandomIsler/Dialogue/DialogueTree")]
    public class DialogueTree : SaveableObject
    {
        [SerializeField] private DialogueNode _firstDialogueNode;
        
        public UnlockCriteria UnlockCriteria;
        public bool HasBeenSeen = false;

        public bool CanDialogueBePlayed => UnlockCriteria.IsUnlocked();

        public DialogueNode GetFirstDialogueNode()
        {
            HasBeenSeen = true;
            return _firstDialogueNode;
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            
            HasBeenSeen = false;
        }

        public override void Load(SOData data)
        {
            var dialogueData = data as DialogueData;
            if (dialogueData == null)
                return;
            
            HasBeenSeen = dialogueData.HasBeenSeen;

            if (!SaveableObjectHelper.Instance.AllDialogueTrees.Contains(this))
                SaveableObjectHelper.Instance.AllDialogueTrees.Add(this);
        }

        public override SOData GetData()
        {
            return new DialogueData()
            {
                ID = ID,
                HasBeenSeen = HasBeenSeen
            };
        }
    }
}
