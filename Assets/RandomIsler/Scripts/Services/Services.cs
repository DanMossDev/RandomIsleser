using MossUtils;
using UnityEngine;

namespace RandomIsleser
{
    public class Services : MonoBehaviour
    {
	    private GameManager _gameManager;
		private ObjectPoolController _objectPoolController;
		private UIManager _uiManager;
		private QuestManager _questManager;
		private DialogueManager _dialogueManager;
        
		public GameManager GameManager => _gameManager;
		public ObjectPoolController ObjectPoolController => _objectPoolController;
		public UIManager UIManager => _uiManager;
		public QuestManager QuestManager => _questManager;
		public DialogueManager DialogueManager => _dialogueManager;
		
		public static Services Instance { get; private set; }

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(gameObject);
				return;
			}
			
			_gameManager = GetComponentInChildren<GameManager>(true);
			_gameManager.gameObject.SetActive(true);
			_objectPoolController = GetComponentInChildren<ObjectPoolController>(true);
			_objectPoolController.gameObject.SetActive(true);
			_uiManager = GetComponentInChildren<UIManager>(true);
			_uiManager.gameObject.SetActive(true);
			_questManager = GetComponentInChildren<QuestManager>(true);
			_questManager.gameObject.SetActive(true);
			_dialogueManager = GetComponentInChildren<DialogueManager>(true);
			_dialogueManager.gameObject.SetActive(true);
		}

		private void OnEnable()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(this);
			}
			else
				Destroy(gameObject);
		}

		private void OnDisable()
		{
			if (Instance == this)
				Instance = null;
		}
	}
}
