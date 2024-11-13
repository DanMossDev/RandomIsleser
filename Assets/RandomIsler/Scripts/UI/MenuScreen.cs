using UnityEngine;

namespace RandomIsleser
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MenuScreen : Screen
    {
        [System.NonSerialized] public CanvasGroup CanvasGroup;
        
        protected override void Awake()
        {
            base.Awake();
            CanvasGroup = GetComponent<CanvasGroup>();
            CanvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }
    }
}
