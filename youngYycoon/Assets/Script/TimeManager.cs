using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace TimeExtension
{
    public class TimeManager : MonoBehaviour
    {
        private static TimeManager m_instance;
        private static readonly object lockObj = new object();
        public static TimeManager Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (lockObj)
                    {
                        // type으로 찾음
                        var findObjects = FindObjectsByType<TimeManager>(FindObjectsSortMode.None);

                        if (findObjects.Length <= 0)
                        {
                            GameObject TimeManagerObject = new GameObject(String.Format("{0}", typeof(TimeManager).FullName));

                            m_instance = TimeManagerObject.AddComponent<TimeManager>();

                            DontDestroyOnLoad(TimeManagerObject);
                        }
                        else
                        {
                            m_instance = findObjects[0];
                        }
                    }
                }

                return m_instance;
            }
        }
        private Dictionary<int, List<Action>> actions = new Dictionary<int, List<Action>>();
        private int tickCount = 0;
        private const int MaxTickCount = int.MaxValue - 1000; // 오버플로우 방지를 위한 최대 값

        private void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;
                DontDestroyOnLoad(gameObject);
                StartTimer();
            }
            else if (m_instance != this)
            {
                Destroy(gameObject);
            }
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

