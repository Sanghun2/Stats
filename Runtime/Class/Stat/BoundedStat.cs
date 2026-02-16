using System;
using UnityEngine;

namespace BilliotGames
{
    public class BoundedStat : Stat, IBoundedValue<float>
    {
        public string ID => id;

        public float CurrentValue => value;
        public float MinValue => _minValue;
        public float MaxValue => _maxValue;
        public override Value<float> Value => new Value<float>(CurrentValue, deltaValue:0, MinValue, MaxValue);

        public override event Action<Value<float>> OnValueChanged;

        [SerializeField] protected float _maxValue;
        [SerializeField] protected float _minValue;

        public BoundedStat(string id) : base(id) {
            this.id = id;
        }

        public BoundedStat(string id, float maxValue) : base(id) {
            this.value = maxValue;
            this._maxValue = maxValue;            
        }

        public BoundedStat(float maxValue) {
            this.value = maxValue;
            this._maxValue = maxValue;
        }

        public override void ChangeValue(float deltaValue) {
            float prevValue = value;
            value += deltaValue;
            float resultValue = Mathf.Clamp(value, _minValue, _maxValue);
            float appliedDelta = resultValue - prevValue;
            value = resultValue;
            OnValueChanged?.Invoke(new Value<float>(value, appliedDelta, _minValue, _maxValue));
        }
    }
}
