using UnityEngine;
using UnityEngine.Localization;

namespace RandomIsleser
{
    public class EquippableModel : ScriptableObject
    {
        [Header("Equippable Settings")] 
        public LocalizedString Name;
        public int ItemIndex;
        public Equippables EquippableType;

        public bool Slottable = true;
        public bool Unlocked = true;//TODO - make this point to save data
        public Sprite Sprite;
    }
}
