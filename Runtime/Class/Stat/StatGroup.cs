using System;
using UnityEngine;

namespace BilliotGames
{
    public class StatGroup
    {
        private Stat currentValueStat;
        private Stat maxValueStat;

        public StatGroup(Stat currentValueStat, Stat maxValueStat) {
            this.currentValueStat = currentValueStat;
            this.maxValueStat = maxValueStat;

            currentValueStat.SetValue(new Value<float>(maxValueStat.ModifiedValue, 0));
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


        public void ChangeCurrentValue(float deltaValue) {
            currentValueStat.ChangeRawValue(deltaValue);
        }
        public void ChangeMaxValue(float deltaValue) {
            maxValueStat.ChangeRawValue(deltaValue);
        }

        public void SetCurrentValue(float value) {
            currentValueStat.SetValue(new Value<float>(value,0));
        }
        public void SetMaxValue(float value) {
            maxValueStat.SetValue(new Value<float>(value, 0));
        }
    }
}
