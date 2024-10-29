using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace TimeExtension
{
    public class TimeManager : MonoSingletonClass<TimeManager>
    {
        private Dictionary<int, List<Action>> actions = new Dictionary<int, List<Action>>();
        private int tickCount = 0;
        private const int MaxTickCount = int.MaxValue - 1000; // 오버플로우 방지를 위한 최대 값

        protected override void Awake()
        {
            base.Awake();
            StartTimer();
        }

        private void StartTimer()
        {
            IDisposable timer = Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(_ => ExecuteActions());
        }

        private void ExecuteActions()
        {
            tickCount++;
            if (tickCount >= MaxTickCount)
            {
                tickCount = 0; // 오버플로우 방지를 위해 초기화
            }

            var keys = new List<int>(actions.Keys); // 키의 복사본 생성

            for (int i = 0; i < keys.Count; i++)
            {
                int key = keys[i];
                if (actions[key] == null || ReferenceEquals(actions[key], null)) continue;

                if (tickCount % key == 0)
                {
                    var actionListCopy = new List<Action>(actions[key]); // 리스트 복사본 생성
                    foreach (var action in actionListCopy)
                    {
                        action?.Invoke();
                    }
                }
            }
        }

        public void RegisterAction(int interval, Action action)
        {
            if (!actions.ContainsKey(interval))
            {
                actions[interval] = new List<Action>();
            }
            actions[interval].Add(action);
        }

        public void UnregisterAction(int interval, Action action)
        {
            if (actions.ContainsKey(interval))
            {
                actions[interval].Remove(action);
                if (actions[interval].Count == 0)
                {
                    actions.Remove(interval);
                }
            }
        }
    }
}

