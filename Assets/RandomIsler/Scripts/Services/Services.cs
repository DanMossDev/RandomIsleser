using MossUtils;
using UnityEngine;

namespace RandomIsleser
{
    public class Services : MonoSingleton<Services>
    {
		private InputManager _inputManager;
		private ObjectPoolController _objectPoolController;
        
		public InputManager InputManager => _inputManager;
		public ObjectPoolController ObjectPoolController => _objectPoolController;

		private void Awake()
		{
			_inputManager = GetComponentInParent<InputManager>();
			_objectPoolController = GetComponentInParent<ObjectPoolController>();
		}
	}
}
