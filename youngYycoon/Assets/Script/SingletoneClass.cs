using UnityEngine;

public class SingletoneClass<T>
        where T : class, new()
{
    static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }

            return _instance;
        }
    }
}

