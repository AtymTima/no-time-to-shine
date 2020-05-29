using UnityEngine;

public class PoolLifetime : MonoBehaviour
{
    private float maxLifeTime = 0.3f;
    private float currentLifeTime;

    ObjectPool<PoolLifetime> particlesPool;

    private void Start()
    {
        particlesPool = ObjectPool<PoolLifetime>.Instance;
    }

    private void OnEnable()
    {
        currentLifeTime = 0f;
    }

    private void Update()
    {
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime >= maxLifeTime)
        {
            particlesPool.ReturnToPool(this);
        }
    }
}
