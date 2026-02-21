using System;
using UnityEngine;

namespace BilliotGames
{
    [Serializable]
    public class Stat
    {
        public virtual Value<float> Value => new Value<float>(value, deltaValue:0);

        [SerializeField] protected string id;
        [SerializeField] protected float value;

        public virtual event Action<Value<float>> OnValueChanged;

        public Stat(string id) {
            this.id = id;
        }

        public Stat(float value) {
            this.value = value;
        }

        public virtual void ChangeValue(float delataValue) {
            value += delataValue;
            OnValueChanged?.Invoke(new Value<float>(value, delataValue));
        }
    }
}
