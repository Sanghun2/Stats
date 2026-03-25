using System;
using System.Collections.Generic;
using BilliotGames;
using UnityEngine;
using static UnityEditor.MaterialProperty;
using static UnityEngine.EventSystems.EventTrigger;

namespace BilliotGames
{
    public class StatContainer
    {
        private Dictionary<string, IStatEntry> statDict = new Dictionary<string, IStatEntry>();
        private bool _isInit;

        public virtual void Init() {
            if (_isInit) return;

            _isInit = true;
        }

        #region Common

        public void ClearStats() {
            statDict.Clear();
        }
        public void RegisterStat(IStatEntry stat) {
            if (!statDict.TryAdd(stat.ID, stat)) {
                Debug.LogError($"<color=red>{stat.ID}는 이미 존재하는 ID여서 stat 추가 실패</color>");
            }
        }

        public Value<float>? GetRawValue(string statID) {
            if (statDict.TryGetValue(statID, out var entry))
                return entry.RawValue;
            Debug.LogError($"{statID} not exist");
            return null;
        }
        public bool TryChangeRawValue(string statID, float deltaValue) {
            if (TryGetStat(statID, out IStatEntry stat)) {
                stat.ChangeRawValue(deltaValue);
                return true;
            }

            Debug.LogError($"<color=red>stat ({statID}) is not exist.</color>");
            return false;
        }


        public bool TryGetStat(string statID, out IStatEntry stat) {
            return statDict.TryGetValue(statID, out stat);
        }
        public void OverrideStat(string statID, IStatEntry overrideStat) {
            statDict[statID] = overrideStat;
        }

        public bool TryOverrideStatValue(string statID, Value<float> overrideValue) {
            if (statDict.TryGetValue(statID, out IStatEntry stat)) {
                stat.SetValue(overrideValue);
                return true;
            }

            Debug.LogError($"<color=red>failed to override stat value. no ({statID}) is exist. reigster first</color>");
            return false;
        }

        #endregion

        #region Stat

        #endregion

        #region Stat Group

        public Value<float>? GetMaxRawValue(string statID) {
            if (statDict.TryGetValue(statID, out IStatEntry stat) && stat is StatGroup group)
                return group.RawMaxValue;
            return null;
        }
        public bool TryChangeRawMaxVale(string statID, float deltaValue) {
            if (TryGetStat(statID, out IStatEntry stat) && stat is StatGroup group) {
                group.ChangeMaxValue(deltaValue);
                return true;
            }

            Debug.LogError($"<color=red>stat ({statID}) is not exist.</color>");
            return false;
        }

        #endregion





        public void RegisterEvent(Action<Value<float>> @event, string statID, string subID = null) {
            if (TryGetStat(statID, out IStatEntry entry)) {
                if (string.IsNullOrEmpty(subID) && entry is Stat stat) {
                    RegisterEvent(@event, stat);
                    return;
                }
                else if (entry is StatGroup group) {
                    RegisterEvent(@event, group, subID);
                }
            }

            Debug.LogError($"<color=red>{statID}에 해당하는 Stat이 없음</color>");
        }
        public void UnregisterEvent(Action<Value<float>> @event, string statID, string subID = null) {
            if (TryGetStat(statID, out IStatEntry entry)) {
                if (string.IsNullOrEmpty(subID) && entry is Stat stat) {
                    UnregisterEvent(@event, stat);
                    return;
                }
                else if (entry is StatGroup group) {
                    UnregisterEvent(@event, group, subID);
                }
            }
        }

        private void RegisterEvent(Action<Value<float>> @event, Stat stat) {
            stat.OnValueChanged -= @event;
            stat.OnValueChanged += @event;
        }
        private void RegisterEvent(Action<Value<float>> @event, StatGroup group, string subID) {
            if (group.TryGetStat(subID, out Stat targetStat)) {
                RegisterEvent(@event, targetStat);
                return;
            }

            Debug.LogError($"<color=red>stat is not exist sub ID of ({subID})</color>");
        }


        private void UnregisterEvent(Action<Value<float>> @event, Stat stat) {
            stat.OnValueChanged -= @event;
        }
        private void UnregisterEvent(Action<Value<float>> @event, StatGroup group, string subID) {
            if (group.TryGetStat(subID, out Stat stat)) {
                UnregisterEvent(@event, stat);
                return;
            }

            Debug.LogError($"<color=red>stat is not exist sub ID of ({subID})</color>");
        }
    }
}
