using System;
using UnityEngine;

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
    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (m_instance != this)
        {
            Destroy(gameObject);
        }
    }

    
}
