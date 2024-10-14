using System;
using UnityEngine;
using UnityEngine.Localization;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "ObjectiveModel", menuName = "RandomIsler/Quests/ObjectiveModel")]
    public class ObjectiveModel : ScriptableObject
    {
        public int ID;

        public bool IsStarted;
        public bool IsComplete;
        
        public LocalizedString ObjectiveName;
        public LocalizedString ObjectiveDescription;

        [SerializeField] private QuestModel _owner;

        public void StartObjective()
        {
            if (IsStarted)
                return;
            
            IsStarted = true;
        }

        public void CompleteObjective()
        {
            if (IsComplete)
                return;
            
            IsComplete = true;
            _owner.ObjectiveCompleted(this);
        }
        
        private void OnValidate()
        {
            if (ID == 0)
                ID = Guid.NewGuid().GetHashCode();

            IsStarted = false;
            IsComplete = false;
        }
        
        public void Load(ObjectiveData data)
        {
            IsStarted = data.IsStarted;
            IsComplete = data.IsComplete;
        }

        public ObjectiveData GetData()
        {
            return new ObjectiveData()
            {
                ID = ID,
                IsStarted = IsStarted,
                IsComplete = IsComplete,
            };
        }
    }
}
