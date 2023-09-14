using UnityEngine;

public class Singleton<T> : MonoBehaviour
    where T : Component
{
    public bool dontDestroy;
    private static T _instance;
    
    public static T Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = (T) FindObjectOfType(typeof(T));
            if (_instance == null)
            {
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (dontDestroy && CheckInstance()) DontDestroyOnLoad(gameObject);
    }

    protected bool CheckInstance()
    {
        if (this == Instance)
        {
            return true;
        }

        Destroy(this);
        return false;
    }
}
