using System;
using UnityEngine;

namespace RandomIsleser
{
    public abstract class ScriptedEvent : MonoBehaviour
    {
        [Header("Scripted Event Settings")] 
        [SerializeField] protected SaveableBool _alreadyCompletedBool;
        
        public event Action OnEventStarted;
        public event Action OnEventCompleted;

        public virtual void BeginEvent()
        {
            OnEventStarted?.Invoke();
        }

        public virtual void EndEvent()
        {
            if (_alreadyCompletedBool != null)
                _alreadyCompletedBool.Value = true;
            
            OnEventCompleted?.Invoke();
        }
    }
}
