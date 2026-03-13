using System;
using System.Collections.Generic;
using BilliotGames;
using UnityEngine;

namespace BilliotGames
{
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
        public void ClearStats() {
            statDict.Clear();
        }


        public Value<float>? GetStatRawValue(string statID) {
            if (TryGetStat(statID, out Stat stat)) {
                return stat.RawValue;
            }

            Debug.LogError($"<color=red>{statID} named stat is not exist.</color>");
            return null;
        }
        public bool TryChangeRawStat(string statID, float deltaValue) {
            if (TryGetStat(statID, out Stat stat)) {
                stat.ChangeRawValue(deltaValue);
                return true;
            }

            Debug.LogError($"<color=red>stat ({statID}) is not exist.</color>");
            return false;
        }
        public bool TryGetStat(string statID, out Stat stat) {
            return statDict.TryGetValue(statID, out stat);
        }
        public void OverrideStat(string statID, Stat overrideStat) {
            statDict[statID] = overrideStat;
        }
        public bool TryOverrideStatValue(string statID, Value<float> overrideValue) {
            if (statDict.TryGetValue(statID, out Stat targetStat)) {
                targetStat.SetValue(overrideValue);
                return true;
            }

            Debug.LogError($"<color=red>failed to override stat value. no ({statID}) is exist. reigster first</color>");
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
    }
}
