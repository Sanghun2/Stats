using System;
using UnityEngine;

namespace BilliotGames
{
    [Serializable]
    public class Stat
    {
        public virtual Value Value => new Value(value, deltaValue:0);

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
