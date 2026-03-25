using System;

namespace BilliotGames
{
    public interface IStatEntry
    {
        string ID { get; }
        Value<float> RawValue { get; }
        float ModifiedValue { get; }
        event Action<Value<float>> OnValueChanged;
        event Action<Value<float>> OnModifierUpdated;
    }

    public interface IStat : IStatEntry
    {
        void ChangeRawValue(float deltaValue);
        void SetValue(Value<float> valueData);
        void AddModifier(StatModifier modifier);
        void RemoveModifier(StatModifier modifier);
    }

    public interface IStatGroup : IStatEntry
    {
        void ChangeCurrentValue(float deltaValue);
        void ChangeMaxValue(float deltaValue);
        void SetCurrentValue(float value);
        void SetMaxValue(float value);
        event Action<Value<float>> OnCurrentValueChanged;
        event Action<Value<float>> OnMaxValueChanged;
    }
}
