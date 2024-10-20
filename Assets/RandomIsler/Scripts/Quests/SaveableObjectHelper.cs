using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "SaveableObjectHelper", menuName = AssetMenuNames.Models + "SaveableObjectHelper")]
    public class SaveableObjectHelper : ScriptableObject
    {
        public List<SaveableObject> AllSaveableObjects;
        
        
        private Dictionary<int, SaveableObject> _allSaveableObjectsDictionary;

        public Dictionary<int, SaveableObject> AllSaveableObjectsDictionary
        {
            get
            {
                if (_allSaveableObjectsDictionary == null)
                {
                    _allSaveableObjectsDictionary = new Dictionary<int, SaveableObject>();
                    foreach (var obj in AllSaveableObjects)
                        _allSaveableObjectsDictionary.Add(obj.ID, obj);
                }

                return _allSaveableObjectsDictionary;
            }
        }
        
        #region Singleton
        private static SaveableObjectHelper _instance;
        public static SaveableObjectHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<SaveableObjectHelper>("Models/SaveableObjectHelper");
                }
                return _instance;
            }
        }
        #endregion

        public void ClearAll()
        {
            AllSaveableObjects.Clear();
        }
    }
}
