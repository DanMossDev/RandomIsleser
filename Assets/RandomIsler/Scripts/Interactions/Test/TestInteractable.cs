using UnityEngine;

namespace RandomIsleser
{
    public class TestInteractable : MonoBehaviour, Interactable
    {
        [SerializeField] private QuestModel _quest;
        [SerializeField] private ObjectiveModel _objective;
        public void Interact()
        {
            if (_quest)
                _quest.BeginQuest();
            else if (_objective)
                _objective.CompleteObjective();
        }
    }
}
