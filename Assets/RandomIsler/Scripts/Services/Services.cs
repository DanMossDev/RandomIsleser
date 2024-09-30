using MossUtils;
using UnityEngine;

namespace RandomIsleser
{
    public class Services : MonoSingleton<Services>
    {
		private InputManager _inputManager;
		private ObjectPoolController _objectPoolController;
		private UIManager _uiManager;
        
		public InputManager InputManager => _inputManager;
		public ObjectPoolController ObjectPoolController => _objectPoolController;
		public UIManager UIManager => _uiManager;

		private void Awake()
		{
			_inputManager = GetComponentInChildren<InputManager>();
			_objectPoolController = GetComponentInChildren<ObjectPoolController>();
			_uiManager = GetComponentInChildren<UIManager>();
		}
	}
}
