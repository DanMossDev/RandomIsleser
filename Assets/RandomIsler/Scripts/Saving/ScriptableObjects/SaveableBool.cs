using System;
using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "SaveableBool", menuName = AssetMenuNames.SaveableData + "SaveableBool")]
    public class SaveableBool : SaveableObject
    {
        private bool _value;

        public bool Value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }

        [SerializeField] private bool _defaultValue;

        public event Action<bool> OnValueChanged;

        public override void Load(SOData data)
        {
            var boolData = data as BoolData;
            if (boolData == null)
                return;
            
            Value = boolData.Value;
        }
        
        public override SOData GetData()
        {
            return new BoolData()
            {
                ID = ID,
                Value = Value
            };
        }
        
        protected override void Cleanup()
        {
            base.Cleanup();

            Value = _defaultValue;
        }
    }

    public class BoolData : SOData
    {
        public bool Value;
    }
}
