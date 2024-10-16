using UnityEngine;
using UnityEngine.Localization;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "DialogueNode", menuName = "RandomIsler/Models/Dialogue Node")]
    public class DialogueNode : SaveableObject
    {
        public LocalizedString Dialogue;
        public UnlockCriteria UnlockCriteria;
        public bool HasBeenSeen = false;

        public bool CanDialogueBePlayed => UnlockCriteria.IsUnlocked();

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

            if (!SaveableObjectHelper.Instance.AllDialogueNodes.Contains(this))
                SaveableObjectHelper.Instance.AllDialogueNodes.Add(this);
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
