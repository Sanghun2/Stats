using System;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] protected string id;
    [SerializeField] protected float value;

    public event Action<float, float> OnValueChanged;

    public Stat (string id) {
        this.id = id;
    }

    public Stat() {

    }

    public virtual void ChangeValue(float delataValue) {
        value += delataValue;
        OnValueChanged?.Invoke(value, delataValue);
    }
}
