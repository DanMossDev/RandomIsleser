using System;
using UnityEngine;

namespace RandomIsleser
{
    public abstract class SaveableObject : ScriptableObject
    {
        public int ID;

        public abstract void Load(SOData data);

        public abstract SOData GetData();
        
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
            
            if (!SaveableObjectHelper.Instance.AllSaveableObjects.Contains(this))
                SaveableObjectHelper.Instance.AllSaveableObjects.Add(this);
        }
    }
    
    [Serializable]
    public class SOData
    {
	    public int ID;
    }
}
