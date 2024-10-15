using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "NPCModel", menuName = "RandomIsler/Models/NPC")]
    public class NPCModel : ScriptableObject
    {
        [Header("NPC Model")] [Space, Header("Character Details")] 
        [SerializeField] private LocalizedString Name;

        public CharacterDialogueModel CharacterDialogueModel;

        public List<ObjectiveModel> StartObjectives;
        public List<ObjectiveModel> CompleteObjectives;
    }
}
