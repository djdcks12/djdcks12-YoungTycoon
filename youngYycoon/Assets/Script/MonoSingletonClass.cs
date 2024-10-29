using UnityEngine;

public class MonoSingletonClass<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lockObj = new object();

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lockObj)
                {
                    var findObjects = FindObjectsByType<T>(FindObjectsSortMode.None);

                    if (findObjects.Length <= 0)
                    {
                        GameObject singletonObject = new GameObject(typeof(T).FullName);
                        _instance = singletonObject.AddComponent<T>();
                        DontDestroyOnLoad(singletonObject);
                    }
                    else
                    {
                        _instance = findObjects[0];
                    }
                }
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
