using UnityEngine;
using UnityEngine.Localization;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "DialogueNode", menuName = "RandomIsleser/Models/Dialogue Node")]
    public class DialogueNode : SaveableObject
    {
        public LocalizedString Dialogue;
        public UnlockCriteria UnlockCriteria;
        public bool HasBeenSeen = false;

        public bool CanDialogueBePlayed => UnlockCriteria.IsUnlocked();

        protected override void OnValidate()
        {
            base.OnValidate();
            
            HasBeenSeen = false;
        }

        public override void Load(SOData data)
        {
            var dialogueData = data as DialogueData;
            if (dialogueData == null)
                return;
            
            HasBeenSeen = dialogueData.HasBeenSeen;

            if (!SaveableObjectHelper.instance.AllDialogueNodes.Contains(this))
                SaveableObjectHelper.instance.AllDialogueNodes.Add(this);
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
