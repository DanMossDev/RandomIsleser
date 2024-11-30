using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "CatchFishObjectiveModel", menuName = AssetMenuNames.Quests + "CatchFishObjectiveModel")]
    public class CatchFishObjectiveModel : ObjectiveModel
    {
        [SerializeField] private AnimalFlags _validFishCatches;
        
        public override void StartObjective()
        {
            if (IsStarted)
                return;
            if (!Owner.IsStarted)
                Owner.BeginQuest(false);
            
            IsStarted = true;
            RuntimeSaveManager.Instance.CurrentSaveSlot.QuestSaveData.QuestUpdated(Owner);
            EventRadio.OnFishCaught += OnFishCaught;
        }
        
        public override void CompleteObjective()
        {
            if (IsComplete)
                return;
            
            IsComplete = true;
            if (HasReward)
                OnCompleteReward.UnlockReward();
            Owner.ObjectiveCompleted(this);
            
            RuntimeSaveManager.Instance.CurrentSaveSlot.QuestSaveData.QuestUpdated(Owner);
            EventRadio.OnFishCaught -= OnFishCaught;
        }

        protected override void RestoreInProgress()
        {
            EventRadio.OnFishCaught += OnFishCaught;
        }

        private void OnFishCaught(AnimalFlags fish)
        {
            if (_validFishCatches == 0 || _validFishCatches.HasFlag(fish))
                CompleteObjective();
        }
    }
}
