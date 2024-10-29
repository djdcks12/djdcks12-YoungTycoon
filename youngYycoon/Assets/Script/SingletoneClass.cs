using UnityEngine;

public class SingletoneClass<T>
        where T : SingletoneClass<T>, new()
{
    static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
                _instance.Init();
            }

            return _instance;
        }
    }

    protected virtual void Init()
    {
    }
}

