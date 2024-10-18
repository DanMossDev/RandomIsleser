using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace RandomIsleser
{
    public enum QuestPopupType
    {
        QuestStarted,
        ObjectiveStarted,
        ObjectiveComplete,
        QuestComplete
    }
    public struct QuestPopup
    {
        public QuestPopupType PopupType;
        public string Title;
        public string Details;
        public QuestPopup(QuestPopupType type, string title, string details)
        {
            PopupType = type;
            Title = title;
            Details = details;
        }
    }
    
    public class QuestPopupUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _popupContainer;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _detailsText;
        
        private float _popupStartXPosition;
        [SerializeField] private float _popupActiveXPosition;
        [SerializeField] private float _popupTweenEnterDuration;
        [SerializeField] private float _popupTweenStayDuration;
        [SerializeField] private float _popupTweenExitDuration;
        
        [SerializeField] private LocalizedString _questStartedString;
        [SerializeField] private LocalizedString _objectiveCompleteString;
        [SerializeField] private LocalizedString _objectiveStartedString;
        [SerializeField] private LocalizedString _questCompleteString;
        
        private Queue<QuestPopup> _questPopupQueue = new Queue<QuestPopup>();
        private Sequence _questTween;
        
        private void OnEnable()
        {
            QuestModel.OnQuestStarted += QuestStarted;
            QuestModel.OnQuestUpdated += QuestUpdated;
            QuestModel.OnQuestComplete += QuestComplete;
            
            _popupStartXPosition = _popupContainer.anchoredPosition.x;
        }
        
        private void OnDisable()
        {
            QuestModel.OnQuestStarted -= QuestStarted;
            QuestModel.OnQuestUpdated -= QuestUpdated;
            QuestModel.OnQuestComplete -= QuestComplete;
        }

        private void TryTween()
        {
            if (_questPopupQueue.Count == 0 || (_questTween.IsActive() && !_questTween.IsComplete()))
                return;
            
            var popup = _questPopupQueue.Dequeue();
            _titleText.text = popup.Title;
            _detailsText.text = popup.Details;
            _questTween = DOTween.Sequence();
            _questTween.Append(_popupContainer.DOAnchorPosX(_popupActiveXPosition, _popupTweenEnterDuration));
            _questTween.AppendInterval(_popupTweenStayDuration);
            _questTween.Append(_popupContainer.DOAnchorPosX(_popupStartXPosition, _popupTweenExitDuration));
            
            _questTween.OnComplete(TryTween);
        }

        private void QuestStarted(QuestModel quest)
        {
            _questPopupQueue.Enqueue(new QuestPopup(QuestPopupType.QuestStarted, quest.QuestName.GetLocalizedString(), _questStartedString.GetLocalizedString()));
            if (quest.ObjectiveIndex == 0)
                _questPopupQueue.Enqueue(new QuestPopup(QuestPopupType.ObjectiveStarted, quest.CurrentObjective.ObjectiveName.GetLocalizedString(), _objectiveStartedString.GetLocalizedString()));
            
            TryTween();
        }

        private void QuestUpdated(QuestModel quest)
        {
            _questPopupQueue.Enqueue(new QuestPopup(QuestPopupType.ObjectiveComplete, quest.LastCompletedObjective.ObjectiveName.GetLocalizedString(), _objectiveCompleteString.GetLocalizedString()));
            _questPopupQueue.Enqueue(new QuestPopup(QuestPopupType.ObjectiveStarted, quest.CurrentObjective.ObjectiveName.GetLocalizedString(), _objectiveStartedString.GetLocalizedString()));
            TryTween();
        }

        private void QuestComplete(QuestModel quest)
        {
            _questPopupQueue.Enqueue(new QuestPopup(QuestPopupType.ObjectiveComplete, quest.LastCompletedObjective.ObjectiveName.GetLocalizedString(), _objectiveCompleteString.GetLocalizedString()));
            _questPopupQueue.Enqueue(new QuestPopup(QuestPopupType.QuestComplete, quest.QuestName.GetLocalizedString(), _questCompleteString.GetLocalizedString()));
            TryTween();
        }
    }
}
