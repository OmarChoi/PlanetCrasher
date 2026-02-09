using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; private set; }

    protected virtual bool IsPersistent => false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = (T)this;

        if (IsPersistent)
        {
            DontDestroyOnLoad(gameObject);
        }

        Initialize();
    }

    private void OnDestroy()
    {
        Cleanup();

        if (Instance == this)
        {
            Instance = null;
        }
    }

    protected virtual void Initialize() { }
    protected virtual void Cleanup() { }
}
