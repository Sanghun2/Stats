using System;
using System.Collections.Generic;
using UnityEngine;

namespace BilliotGames
{
    [Serializable]
    public class Stat : IStat
    {
        public string ID => id;

        public virtual Value<float> RawValue => new Value<float>(rawValue, deltaValue: 0);
        public virtual Value<float> ModifiedValue
        {
            get
            {
                return new Value<float>(modifiedValue, 0);
            }
        }


        [SerializeField] protected string id;
        [SerializeField] protected float rawValue;

        private List<StatModifier> modifierList = new();
        protected float modifiedValue;

        public virtual event Action<Value<float>> OnValueChanged;
        public virtual event Action<Value<float>> OnModifierUpdated;

        public Stat(string id) {
            this.id = id;
        }

        public Stat(string id, float baseValue) :this(id) {
            this.rawValue = baseValue;
            this.modifiedValue = baseValue;
        }

        public virtual void ChangeRawValue(float delataValue) {
            rawValue += delataValue;
            OnValueChanged?.Invoke(new Value<float>(rawValue, delataValue));
        }
        public virtual void SetValue(Value<float> valueData) {
            rawValue = valueData.CurrentValue;
            OnValueChanged?.Invoke(valueData);
        }

        #region Modifiers

        public void AddModifier(StatModifier modifier) {
            modifierList.Add(modifier);
            UpdateFinalValue();
        }
        public void RemoveModifier(StatModifier targetModifier) {
            if (targetModifier == null) return;

            for (int i = modifierList.Count - 1; i >= 0; --i) {
                var modifier = modifierList[i];
                if (modifier.Equals(targetModifier)) {
                    modifierList.RemoveAt(i);
                }
            }

            UpdateFinalValue();
        }


        private float CalculateFinalValue(float value) {
            for (int i = 0; i < modifierList.Count; ++i) {
                var modifier = modifierList[i];
                value = modifier.ApplyValue(value);
            }

            return value;
        }
        private void UpdateFinalValue() {
            var prevValue = modifiedValue;
            modifiedValue = CalculateFinalValue(rawValue);
            OnModifierUpdated?.Invoke(new Value<float>(modifiedValue, modifiedValue - prevValue));
        }

        #endregion
    }
}
