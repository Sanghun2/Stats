using System;

namespace BilliotGames
{
    public interface IStatEntry
    {
        string ID { get; }
        Value<float> RawValue { get; }
        Value<float> ModifiedValue { get; }
        event Action<Value<float>> OnModifierUpdated; 
        void ChangeRawValue(float deltaValue);
        void SetValue(Value<float> overrideValue);
    }

    public interface IStat : IStatEntry
    {
        event Action<Value<float>> OnValueChanged;
        void AddModifier(StatModifier modifier);
        void RemoveModifier(StatModifier modifier);
    }

    public interface IStatGroup : IStatEntry
    {
        void ChangeCurrentValue(float deltaValue);
        void ChangeRawMaxValue(float deltaValue);
        void SetCurrentValue(Value<float> value);
        void SetMaxValue(Value<float> value);

        event Action<Value<float>> OnCurrentValueChanged;
        event Action<Value<float>> OnMaxValueChanged;
    }
}
