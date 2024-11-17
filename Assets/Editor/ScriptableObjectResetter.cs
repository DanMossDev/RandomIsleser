#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace RandomIsleser.Editor
{
    [InitializeOnLoad]
    public static class ScriptableObjectResetter
    {
        static ScriptableObjectResetter()
        {
            EditorApplication.playModeStateChanged += CleanupSaveableObjects;
        }

        private static void CleanupSaveableObjects(PlayModeStateChange state)
        {
            CleanupSaveableObjects();
        }

        [MenuItem("MossUtils/Cleanup Saveable Objects")]
        private static void CleanupSaveableObjects()
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(SaveableObject)}"); 
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                AssetDatabase.LoadAssetAtPath<SaveableObject>(path).OnForceCleanup();
            }
        }
    }
}
#endif