using System;
using System.Collections.Generic;
using BilliotGames;
using UnityEngine;

public class StatContainer
{
    private Dictionary<string, Stat> statDict = new Dictionary<string, Stat>();
    private bool _isInit;

    public virtual void Init() {
        if (_isInit) return;

        _isInit = true;
    }

    public void RegisterEvent(string statID, Action<Value> @event) {
        if (TryGetStat(statID, out Stat stat)) {
            stat.OnValueChanged -= @event;
            stat.OnValueChanged += @event;
        }
        else {
            Debug.LogError($"{statID}에 해당하는 Stat이 없음");
        }
    }
    public void UnregisterEvet(string statID, Action<Value> @event) {
        if (TryGetStat(statID, out Stat stat)) {
            stat.OnValueChanged -= @event;
        }
    }

    public Value? GetStatValue(string statID) {
        if (TryGetStat(statID, out Stat stat)) {
            return stat.Value;
        }

        Debug.LogError($"<color=red>{statID} named stat not exist.</color>");
        return null;
    }
    public void ChangeStat(string statID, float deltaValue) {
        if (TryGetStat(statID, out Stat stat)) {
            stat.ChangeValue(deltaValue);
        }
    }

    private bool TryGetStat(string statID, out Stat stat) {
        return statDict.TryGetValue(statID, out stat);
    }
}
