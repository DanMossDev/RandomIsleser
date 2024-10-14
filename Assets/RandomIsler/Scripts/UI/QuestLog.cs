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

        public void Populate(QuestData questData)
        {
            _questTitle.text = questData.QuestName.GetLocalizedString();
            _questDescription.text = questData.QuestDescription.GetLocalizedString();
            //_objectiveTitle.text = questData.ObjectiveName.GetLocalizedString();
            //_objectiveDescription.text = questData.ObjectiveDescription.GetLocalizedString();
        }
    }
}
