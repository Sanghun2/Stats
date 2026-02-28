using System;
using UnityEngine;

namespace BilliotGames
{
    [Serializable]
    public class StatModifier : IEquatable<StatModifier>
    {
        public enum ModifierType {
            PureAdd,
            PureMultiply,
            PercentAdd,
        }

        public string ModifierID => modifierID;

        private string modifierID;
        private float modifierValue;
        private ModifierType modifierType;

        public float ApplyValue(float targetValue) {
            return ModifyValue(targetValue, modifierValue, modifierType);
        }

        private float ModifyValue(float targetValue, float modifierValue,  ModifierType modifierType) {
            switch (modifierType) {
                case ModifierType.PureAdd:
                    return targetValue + modifierValue;
                case ModifierType.PureMultiply:
                    return targetValue * modifierValue;
                case ModifierType.PercentAdd:
                    return targetValue * (1 + modifierValue);
                default:
                    Debug.LogError($"<color=red>No mod type exist</color>");
                    return targetValue;
            }
        }

        public bool Equals(StatModifier other) {
            if (other == null) return false;
            if (!modifierID.Equals(other.modifierID)) return false;
            if (!Mathf.Approximately(modifierValue, other.modifierValue)) return false;
            if (modifierType != other.modifierType) return false;
            return true;
        }
    }
}
