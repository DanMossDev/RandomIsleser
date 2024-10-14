using System;
using UnityEngine.Localization;

namespace RandomIsleser
{
    [Serializable]
    public class QuestData
    {
        public LocalizedString QuestName;
        public LocalizedString QuestDescription;
        public bool IsStarted = false;
        public bool IsComplete = false;

        // public LocalizedString ObjectiveName => CurrentObjective.ObjectiveName;
        // public LocalizedString ObjectiveDescription => CurrentObjective.ObjectiveDescription;
    }
}
