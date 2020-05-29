using UnityEngine;

public class OutlineLifeTime : MonoBehaviour
{
    private float maxLifeTime = 0.6f;
    private float currentLifeTime;

    ObjectPool<OutlineLifeTime> particlesPool;

    private void Start()
    {
        particlesPool = ObjectPool<OutlineLifeTime>.Instance;
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
