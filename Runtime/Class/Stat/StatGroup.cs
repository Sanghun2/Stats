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
        public float ModifiedValue => currentValueStat.ModifiedValue;
        public Value<float> RawMaxValue => maxValueStat.RawValue;
        public float ModifiedMaxValue => maxValueStat.ModifiedValue;


        private string groupID;
        private Stat currentValueStat;
        private Stat maxValueStat;

        public event Action<Value<float>> OnModifierUpdated;

        public StatGroup(string groupID, BoundedStat currentValueStat, Stat maxValueStat) {
            this.groupID = groupID;
            this.currentValueStat = currentValueStat;
            this.maxValueStat = maxValueStat;

            currentValueStat.SetValue(new Value<float>(ModifiedMaxValue, 0, 0, ModifiedMaxValue));
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
        public void ChangeMaxValue(float deltaValue) {
            maxValueStat.ChangeRawValue(deltaValue);
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
