using UnityEngine;

namespace RandomIsleser
{
    public class TestInteractable : MonoBehaviour, Interactable
    {
        [SerializeField] private QuestModel _quest;
        public void Interact()
        {
            Debug.Log("Interacting");
            _quest.BeginQuest();
        }
    }
}
