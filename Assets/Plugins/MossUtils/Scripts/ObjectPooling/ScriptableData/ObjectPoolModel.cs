using System.Collections.Generic;
using UnityEngine;

namespace MossUtils
{
    [CreateAssetMenu(menuName="MossUtils/Models/ObjectPool", fileName="ObjectPoolModel")]
    public class ObjectPoolModel : ScriptableObject
    {
        public List<string> PrefabNames;
        public List<GameObject> Prefabs;
    }
}
