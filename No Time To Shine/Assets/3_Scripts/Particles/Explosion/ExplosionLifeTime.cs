using UnityEngine;

public class ExplosionLifeTime : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionParticles;
    private float maxLifeTime = 1f;
    private float currentLifeTime;

    ObjectPool<ExplosionLifeTime> explosionPool;

    private void Start()
    {
        explosionPool = ObjectPool<ExplosionLifeTime>.Instance;
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
            explosionPool.ReturnToPool(this);
        }
    }
}
