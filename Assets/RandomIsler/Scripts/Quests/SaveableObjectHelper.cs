using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "SaveableObjectHelper", menuName = "RandomIsler/Quests/SaveableObjectHelper")]
    public class SaveableObjectHelper : ScriptableObject
    {
        public List<QuestModel> AllQuests;
        public List<ObjectiveModel> AllObjectives;
        public List<DialogueTree> AllDialogueTrees;
        
        
        
        #region Singleton
        private static SaveableObjectHelper _instance;
        public static SaveableObjectHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<SaveableObjectHelper>("Models/SaveableObjectHelper");
                }
                return _instance;
            }
        }
        #endregion

        public void ClearAll()
        {
            AllQuests.Clear();
            AllObjectives.Clear();
            AllDialogueTrees.Clear();
        }
    }
}
