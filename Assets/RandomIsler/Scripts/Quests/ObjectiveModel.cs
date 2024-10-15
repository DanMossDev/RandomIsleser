using System;
using UnityEngine;
using UnityEngine.Localization;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "ObjectiveModel", menuName = "RandomIsler/Quests/ObjectiveModel")]
    public class ObjectiveModel : SaveableObject
    {
        public QuestModel Owner;

        public bool IsStarted;
        public bool IsComplete;
        
        public LocalizedString ObjectiveName;
        public LocalizedString ObjectiveDescription;

        [SerializeField] private ObjectiveModel _prerequisiteObjective;
        
        public bool CanBeStarted => (Owner.IsStarted || Owner.CanBeStarted) && (_prerequisiteObjective == null || _prerequisiteObjective.IsComplete);

        public void StartObjective()
        {
            if (IsStarted)
                return;
            if (!Owner.IsStarted)
                Owner.BeginQuest();
            
            IsStarted = true;
        }

        public void CompleteObjective()
        {
            if (IsComplete)
                return;
            
            IsComplete = true;
            Owner.ObjectiveCompleted(this);
        }
        
        protected override void OnValidate()
        {
            base.OnValidate();

            IsStarted = false;
            IsComplete = false;
            
            if (!SaveableObjectHelper.instance.AllObjectives.Contains(this))
                SaveableObjectHelper.instance.AllObjectives.Add(this);
        }
        
        public override void Load(SOData data)
        {
            var objData = data as ObjectiveData;
            if (objData == null)
                return;
            IsStarted = objData.IsStarted;
            IsComplete = objData.IsComplete;
        }

        public override SOData GetData()
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
