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

        public new event Action<float, float, float, float> OnValueChanged;

        [SerializeField] protected float maxValue;
        [SerializeField] protected float minValue;

        public BoundedStat(string id) : base(id) {
            this.id = id;
        }

        public BoundedStat(string id, float maxValue) : base(id) {
            this.maxValue = maxValue;            
        }

        public BoundedStat(float maxValue){
            this.maxValue = maxValue;
        }

        public override void ChangeValue(float delataValue) {
            float prevValue = value;
            value += delataValue;
            float resultValue = Mathf.Clamp(value, minValue, maxValue);
            float appliedDelta = resultValue - prevValue;
            value = resultValue;
            OnValueChanged?.Invoke(value, appliedDelta, minValue, maxValue);
        }
    }
}
