using UnityEngine;

public abstract class SingletonModule<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance => instance;

    // Overridable flag
    protected virtual bool DontDestroy => false;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;

            if (DontDestroy)
                DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}
