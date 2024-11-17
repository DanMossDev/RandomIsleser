using System;
using UnityEngine;

namespace RandomIsleser
{
    public abstract class SaveableObject : ScriptableObject
    {
        public int ID;
        [HideInInspector] public string Name;

        public virtual void Save()
        {
            RuntimeSaveManager.Instance.CurrentSaveSlot.ScriptableObjectData[ID] = GetData();
        }
        
        public abstract void Load(SOData data);

        public abstract SOData GetData();
        
#if UNITY_EDITOR
        protected void OnValidate()
        {
            Cleanup();
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void OnForceCleanup()
        {
            Cleanup();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif

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
        public string Name;
    }
}
