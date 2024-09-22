using UnityEngine;

namespace MossUtils
{
    public class MonoSingleton<T> : MonoBehaviour
    {
        public static bool IsEnabled;
        public static T Instance;
        
        protected void TrySetInstance()
        {
            if (Instance == null)
                Instance = GetComponent<T>();
            else
            {
                Debug.Log(typeof(T) + " already instantiated");
                //Destroy(gameObject); TODO- Uncomment this when no longer using the MovementTest scene
            }
            IsEnabled = true;
        }

        protected void OnEnable()
        {
            TrySetInstance();
        }

        protected void OnDisable()
        {
            Instance = default(T);
        }
    }
}
