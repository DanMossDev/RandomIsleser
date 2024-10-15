using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    [System.Serializable]
    public class UnlockCriteria
    {
        [SerializeField] private List<QuestModel> _unlockingQuests;
        [SerializeField] private List<ObjectiveModel> _unlockingObjectives;
        
        //TODO add some "MilestoneReached enums to game and check against them here

        public bool IsUnlocked()
        {
            if (_unlockingQuests.Count > 0)
                return CheckQuestUnlocked();

            if (_unlockingObjectives.Count > 0)
                return CheckObjectiveUnlocked();
            return false;
        }

        private bool CheckQuestUnlocked()
        {
            foreach (var quest in _unlockingQuests)
            {
                if (!quest.IsComplete)
                {
                    return false;
                }
            }

            return true;
        }
        
        private bool CheckObjectiveUnlocked()
        {
            foreach (var obj in _unlockingObjectives)
            {
                if (!obj.IsComplete)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
