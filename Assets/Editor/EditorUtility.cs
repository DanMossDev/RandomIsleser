using System.IO;
using MossUtils;
using UnityEditor;
using UnityEngine;

namespace RandomIsleser.Editor
{
    [ExecuteInEditMode]
    public class EditorUtility : MonoBehaviour
    {
        [MenuItem("MossUtils/Clear Save Data")]
        private static void ClearSaveData()
        {
            File.Delete(SaveHandlerPersistent.kSaveFilePath);
        }
    }
}
