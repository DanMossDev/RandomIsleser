using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "ProgressItemModel", menuName = "RandomIsler/Models/Items/ProgressItemModel")]
    public class ProgressItemModel : ItemModel
    {
        [SerializeField] private Obstacles _canNavigate;
        
        public Obstacles CanNavigate => _canNavigate;
    }
}
