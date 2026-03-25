using System;
using UnityEngine;

namespace BilliotGames
{
    public class BoundedStat : Stat, IBoundedValue<float>
    {
        public float CurrentValue => cachedFinalValue;
        public float MinValue => _minValue;
        public float MaxValue => _maxValue;
        public override Value<float> RawValue => new Value<float>(CurrentValue, deltaValue:0, MinValue, MaxValue);
        public override Value<float> ModifiedValue => new Value<float>(cachedFinalValue, deltaValue:0, MinValue, MaxValue);

        public override event Action<Value<float>> OnValueChanged;

        [SerializeField] protected float _maxValue;
        [SerializeField] protected float _minValue;

        public BoundedStat(string id) : base(id) {
            this.id = id;
        }

        public BoundedStat(string id, float maxValue) : base(id) {
            this.rawValue = maxValue;
            this._maxValue = maxValue;            
        }

        public BoundedStat(string id, float minValue, float maxValue) : base(id, maxValue) {
            this.rawValue = maxValue;
            this._maxValue = maxValue;
            this._minValue = minValue;
        }

        public override void ChangeRawValue(float deltaValue) {
            float prevValue = rawValue;
            rawValue += deltaValue;
            float resultValue = Mathf.Clamp(rawValue, _minValue, _maxValue);
            float appliedDelta = resultValue - prevValue;
            rawValue = resultValue;
            OnValueChanged?.Invoke(new Value<float>(rawValue, appliedDelta, MinValue, MaxValue));
        }

        public override void SetValue(Value<float> valueData) {
            base.SetValue(valueData);
            _maxValue = valueData.MaxValue;
            _minValue = valueData.MinValue;
            OnValueChanged?.Invoke(valueData);
        }

        internal void SetMaxValue(float maxValue) {
            _maxValue = maxValue;
            OnValueChanged?.Invoke(ModifiedValue);
        }
        public void SetMinValue(float minValue) {
            _minValue = minValue;
            OnValueChanged?.Invoke(ModifiedValue);
        }
    }
}
