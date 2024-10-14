using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "QuestSerialisationHelper", menuName = "RandomIsler/Quests/QuestSerialisationHelper")]
    public class QuestSerialisationHelper : ScriptableObject
    {
        public List<QuestModel> AllQuests;
        public List<ObjectiveModel> AllObjectives;
    }
}
