using System;
using UnityEngine.Localization;

namespace RandomIsleser
{
    [Serializable]
    public class QuestData
    {
        public LocalizedString QuestName;
        public LocalizedString QuestDescription;
        public bool IsComplete = false;
    }
}
