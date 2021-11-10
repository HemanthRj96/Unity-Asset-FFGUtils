using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T instance;
    [SerializeField]
    private bool doNotDestroyOnLoad = true;

    protected void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            if (doNotDestroyOnLoad)
                DontDestroyOnLoad(this);
            instance = this as T;
        }
    }
}
