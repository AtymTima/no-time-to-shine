using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private ObjectPool<PoolLifetime> particlesPool;
    public static PoolManager poolInstance;

    private void Awake()
    {
        //SetUpSingleton();
    }

    private void Start()
    {
        particlesPool = ObjectPool<PoolLifetime>.Instance;
        particlesPool.AddObjects(5);
    }

    private void SetUpSingleton()
    {
        if (poolInstance == null)
        {
            poolInstance = this;
        }
        else
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            particlesPool.AddObjects(5);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
