using System;
using UnityEngine;

namespace RandomIsleser
{
    public class SaveableObject : ScriptableObject
    {
        public int ID;
        
        public virtual void Load(SOData data)
        { }

        public virtual SOData GetData()
        { return null; }
        
        protected void OnValidate()
        {
#if UNITY_EDITOR
            Cleanup();
#endif
        }

        protected virtual void Cleanup()
        {
            if (ID == 0)
                ID = Guid.NewGuid().GetHashCode();
        }
    }
}
