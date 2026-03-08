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


    public void RegisterStat(string id, Stat stat) {
        if (!statDict.TryAdd(id, stat)) {
            Debug.LogError($"<color=red>{id}는 이미 존재하는 ID여서 stat 추가 실패</color>");
        }
    }
    public void Clear() {
        statDict.Clear();
    }

    public Value<float>? GetStatValue(string statID) {
        if (TryGetStat(statID, out Stat stat)) {
            return stat.RawValue;
        }

        Debug.LogError($"<color=red>{statID} named stat is not exist.</color>");
        return null;
    }
    public bool TryChangeStat(string statID, float deltaValue) {
        if (TryGetStat(statID, out Stat stat)) {
            stat.ChangeRawValue(deltaValue);
            return true;
        }

        Debug.LogError($"<color=red>stat ({statID}) is not exist.</color>");
        return false;
    }

    public void RegisterEvent(string statID, Action<Value<float>> @event) {
        if (TryGetStat(statID, out Stat stat)) {
            stat.OnValueChanged -= @event;
            stat.OnValueChanged += @event;
        }
        else {
            Debug.LogError($"{statID}에 해당하는 Stat이 없음");
        }
    }
    public void UnregisterEvent(string statID, Action<Value<float>> @event) {
        if (TryGetStat(statID, out Stat stat)) {
            stat.OnValueChanged -= @event;
        }
    }


    public bool TryGetStat(string statID, out Stat stat) {
        return statDict.TryGetValue(statID, out stat);
    }
}
