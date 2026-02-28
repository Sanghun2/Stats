using System;
using System.Collections.Generic;
using UnityEngine;

namespace BilliotGames
{
    [Serializable]
    public class Stat
    {
        public virtual Value<float> Value => new Value<float>(FinalValue, deltaValue:0);
        public virtual float FinalValue
        {
            get
            {
                if (modifierDirty) {
                    cachedFinalValue = CalculateFinalValue(value);
                    modifierDirty = false;
                }

                return cachedFinalValue;
            }
        }

        [SerializeField] protected string id;
        [SerializeField] protected float value;

        private List<StatModifier> modifierList = new();
        private bool modifierDirty = true;
        private float cachedFinalValue;

        public virtual event Action<Value<float>> OnValueChanged;

        public Stat(string id) {
            this.id = id;
        }

        public Stat(float baseValue) {
            this.value = baseValue;
        }

        public virtual void ChangeValue(float delataValue) {
            value += delataValue;
            OnValueChanged?.Invoke(new Value<float>(value, delataValue));
        }

        #region Modifiers

        public void AddModifier(StatModifier modifier) {
            modifierList.Add(modifier);
            modifierDirty = true;
        }
        public void RemoveModifier(StatModifier targetModifier) {
            if (targetModifier == null) return;

            for (int i = modifierList.Count-1; i >= 0 ; --i) {
                var modifier = modifierList[i];
                if (modifier.Equals(targetModifier)) {
                    modifierList.RemoveAt(i);
                }
            }

            modifierDirty = true;
        }


        private float CalculateFinalValue(float value) {
            for (int i = 0; i < modifierList.Count; ++i) {
                var modifier = modifierList[i];
                value = modifier.ApplyValue(value);
            }

            return value;
        }

        #endregion
    }
}
