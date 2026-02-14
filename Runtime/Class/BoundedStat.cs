using System;
using UnityEngine;

namespace BilliotGames
{
    public class BoundedStat : Stat, IBoundedValue
    {
        public string ID => id;

        public float CurrentValue => value;
        public float MinValue => minValue;
        public float MaxValue => maxValue;

        public override event Action<Value> OnValueChanged;

        [SerializeField] protected float maxValue;
        [SerializeField] protected float minValue;

        public BoundedStat(string id) : base(id) {
            this.id = id;
        }

        public BoundedStat(string id, float maxValue) : base(id) {
            this.value = maxValue;
            this.maxValue = maxValue;            
        }

        public BoundedStat(float maxValue) {
            this.value = maxValue;
            this.maxValue = maxValue;
        }

        public override void ChangeValue(float deltaValue) {
            float prevValue = value;
            value += deltaValue;
            float resultValue = Mathf.Clamp(value, minValue, maxValue);
            float appliedDelta = resultValue - prevValue;
            value = resultValue;
            OnValueChanged?.Invoke(new Value(value, appliedDelta, minValue, maxValue));
        }
    }
}
