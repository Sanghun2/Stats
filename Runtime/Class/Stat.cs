using System;
using UnityEngine;

namespace BilliotGames
{
    [Serializable]
    public class Stat
    {
        [SerializeField] protected string id;
        [SerializeField] protected float value;

        public virtual event Action<Value> OnValueChanged;

        public Stat(string id) {
            this.id = id;
        }

        public Stat() {

        }

        public virtual void ChangeValue(float delataValue) {
            value += delataValue;
            OnValueChanged?.Invoke(new Value(value, delataValue));
        }
    }
}
