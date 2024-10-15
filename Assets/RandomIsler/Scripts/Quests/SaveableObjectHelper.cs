using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "SaveableObjectHelper", menuName = "RandomIsler/Quests/SaveableObjectHelper")]
    public class SaveableObjectHelper : ScriptableSingleton<SaveableObjectHelper>
    {
        public List<QuestModel> AllQuests;
        public List<ObjectiveModel> AllObjectives;
        public List<DialogueNode> AllDialogueNodes;
    }
}
