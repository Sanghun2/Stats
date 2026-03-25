using System;
using System.Collections.Generic;
using UnityEngine;

namespace BilliotGames
{
    public class StatGroup : IStatGroup
    {
        public const string CURRENT_VALUE = "current";
        public const string MAX_VALUE = "max";

        public string ID => groupID;

        public Value<float> RawValue => currentValueStat.RawValue;
        public Value<float> ModifiedValue => currentValueStat.ModifiedValue;
        public Value<float> RawMaxValue => maxValueStat.RawValue;
        public Value<float> ModifiedMaxValue => maxValueStat.ModifiedValue;


        private string groupID;
        private BoundedStat currentValueStat;
        private Stat maxValueStat;

        public event Action<Value<float>> OnModifierUpdated;

        public StatGroup(string groupID, BoundedStat currentValueStat, Stat maxValueStat) {
            this.groupID = groupID;
            this.currentValueStat = currentValueStat;
            this.maxValueStat = maxValueStat;

            var maxValue = maxValueStat.ModifiedValue.CurrentValue;
            this.currentValueStat.SetValue(new Value<float>(maxValue, 0, 0, maxValue));
        }


        public event Action<Value<float>> OnCurrentValueChanged
        {
            add => currentValueStat.OnValueChanged += value;
            remove => currentValueStat.OnValueChanged -= value;
        }

        public event Action<Value<float>> OnMaxValueChanged
        {
            add => maxValueStat.OnValueChanged += value;
            remove => maxValueStat.OnValueChanged -= value;
        }


        public void ChangeRawValue(float deltaValue) {
            ChangeCurrentValue(deltaValue);
        }
        public void ChangeCurrentValue(float deltaValue) {
            currentValueStat.ChangeRawValue(deltaValue);
        }
        public void ChangeRawMaxValue(float deltaValue) {
            maxValueStat.ChangeRawValue(deltaValue);

            var maxValue = maxValueStat.ModifiedValue.CurrentValue;
            var currentValue = currentValueStat.ModifiedValue.CurrentValue;
            var resultValue = Mathf.Min(currentValue, maxValue);
            currentValueStat.SetValue(new Value<float>(resultValue, deltaValue:resultValue - currentValue, 0, maxValue));
        }

        public bool TryGetStat(string subID, out Stat stat) {
            if (subID.Equals(CURRENT_VALUE)) {
                stat = currentValueStat;
            }
            else if (subID.Equals(MAX_VALUE)) {
                stat = maxValueStat;
            }
            else {
                stat = null;
                return false;
            }

            return true;
        }

        public void SetValue(Value<float> overrideValue) {
            SetCurrentValue(overrideValue);
        }
        public void SetCurrentValue(Value<float> value) {
            currentValueStat.SetValue(value);
        }
        public void SetMaxValue(Value<float> value) {
            maxValueStat.SetValue(value);
        }

    }
}
