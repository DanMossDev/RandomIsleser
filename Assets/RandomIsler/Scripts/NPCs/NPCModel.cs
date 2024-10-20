using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "NPCModel", menuName = AssetMenuNames.Models + "NPC")]
    public class NPCModel : ScriptableObject
    {
        [Header("NPC Model")] [Space, Header("Character Details")] 
        public LocalizedString Name;

        public CharacterDialogueModel CharacterDialogueModel;

        public List<ObjectiveModel> StartObjectives;
        public List<ObjectiveModel> CompleteObjectives;

        public NPCController Controller;
    }
}
