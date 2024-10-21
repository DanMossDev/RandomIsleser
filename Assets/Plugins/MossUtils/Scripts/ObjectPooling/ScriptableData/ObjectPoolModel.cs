using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace MossUtils
{
    [CreateAssetMenu(menuName="MossUtils/Models/ObjectPool", fileName="ObjectPoolModel")]
    public class ObjectPoolModel : ScriptableObject
    {
        [SerializedDictionary] public SerializedDictionary<string, GameObject> PrefabLookup;
    }
}
