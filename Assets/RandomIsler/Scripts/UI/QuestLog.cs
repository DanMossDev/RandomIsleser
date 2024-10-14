using TMPro;
using UnityEngine;

namespace RandomIsleser
{
    public class QuestLog : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _questTitle;
        [SerializeField] private TextMeshProUGUI _questDescription;
        [SerializeField] private TextMeshProUGUI _objectiveTitle;
        [SerializeField] private TextMeshProUGUI _objectiveDescription;

        public void Populate(QuestModel data)
        {
            _questTitle.text = data.QuestName.GetLocalizedString();
             _questDescription.text = data.QuestDescription.GetLocalizedString();
            _objectiveTitle.text = data.CurrentObjectiveName.GetLocalizedString();
            _objectiveDescription.text = data.CurrentObjectiveDescription.GetLocalizedString();
        }
    }
}
