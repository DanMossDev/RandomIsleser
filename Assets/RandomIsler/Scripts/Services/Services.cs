using MossUtils;

namespace RandomIsleser
{
    public class Services : MonoSingleton<Services>
    {
	    private GameManager _gameManager;
	    private RuntimeSaveManager _runtimeSaveManager;
		private InputManager _inputManager;
		private ObjectPoolController _objectPoolController;
		private UIManager _uiManager;
		private QuestManager _questManager;
        
		public GameManager GameManager => _gameManager;
		public RuntimeSaveManager RuntimeSaveManager => _runtimeSaveManager;
		public InputManager InputManager => _inputManager;
		public ObjectPoolController ObjectPoolController => _objectPoolController;
		public UIManager UIManager => _uiManager;
		public QuestManager QuestManager => _questManager;

		private void Awake()
		{
			_gameManager = GetComponentInChildren<GameManager>();
			_runtimeSaveManager = GetComponentInChildren<RuntimeSaveManager>();
			_inputManager = GetComponentInChildren<InputManager>();
			_objectPoolController = GetComponentInChildren<ObjectPoolController>();
			_uiManager = GetComponentInChildren<UIManager>();
			_questManager = GetComponentInChildren<QuestManager>();
		}
	}
}
