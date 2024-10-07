using MossUtils;

namespace RandomIsleser
{
    public class Services : MonoSingleton<Services>
    {
	    private GameManager _gameManager;
		private InputManager _inputManager;
		private ObjectPoolController _objectPoolController;
		private UIManager _uiManager;
        
		public GameManager GameManager => _gameManager;
		public InputManager InputManager => _inputManager;
		public ObjectPoolController ObjectPoolController => _objectPoolController;
		public UIManager UIManager => _uiManager;

		private void Awake()
		{
			_gameManager = GetComponentInChildren<GameManager>();
			_inputManager = GetComponentInChildren<InputManager>();
			_objectPoolController = GetComponentInChildren<ObjectPoolController>();
			_uiManager = GetComponentInChildren<UIManager>();
		}
	}
}
