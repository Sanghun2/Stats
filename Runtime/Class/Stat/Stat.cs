using System;
using System.Collections.Generic;
using UnityEngine;

namespace BilliotGames
{
    [Serializable]
    public class Stat
    {
        public virtual Value<float> RawValue => new Value<float>(value, deltaValue: 0);
        public virtual float ModifiedValue
        {
            get
            {
                return cachedFinalValue;
            }
        }


        [SerializeField] protected string id;
        [SerializeField] protected float value;

        private List<StatModifier> modifierList = new();
        private float cachedFinalValue;

        public virtual event Action<Value<float>> OnValueChanged;
        public virtual event Action<Value<float>> OnModifierUpdated;

        public Stat(string id) {
            this.id = id;
        }

        public Stat(float baseValue) {
            this.value = baseValue;
            this.cachedFinalValue = baseValue;
        }

        public virtual void ChangeRawValue(float delataValue) {
            value += delataValue;
            OnValueChanged?.Invoke(new Value<float>(value, delataValue));
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
            var prevValue = cachedFinalValue;
            cachedFinalValue = CalculateFinalValue(value);
            OnModifierUpdated?.Invoke(new Value<float>(cachedFinalValue, cachedFinalValue - prevValue));
        }

        #endregion
    }
}
