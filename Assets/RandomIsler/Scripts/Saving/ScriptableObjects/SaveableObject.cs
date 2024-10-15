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
        
        protected virtual void OnValidate()
        {
            if (ID == 0)
                ID = Guid.NewGuid().GetHashCode();
        }
    }
}
