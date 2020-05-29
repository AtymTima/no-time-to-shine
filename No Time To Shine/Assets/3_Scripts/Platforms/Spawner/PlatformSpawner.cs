using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] int spawnerIndex;
    PlatformCollision fallingPlatform;
    OutlineLifeTime outlineParticles;
    Vector2 spawnPosition;

    ObjectPool<PlatformCollision> objectPool;
    ObjectPool<OutlineLifeTime> particlesPool;

    private void Awake()
    {
        PlatformCollision.onPlatformDeleted += SpawnPlatform;
    }

    private void Start()
    {
        objectPool = ObjectPool<PlatformCollision>.Instance;
        particlesPool = ObjectPool<OutlineLifeTime>.Instance;
    }

    private void OnDestroy()
    {
        PlatformCollision.onPlatformDeleted -= SpawnPlatform;
    }

    private void SpawnPlatform(Transform previousTransform, int platformIndex)
    {
        if (platformIndex != spawnerIndex) { return; }
        fallingPlatform = objectPool.Get();
        spawnPosition.x = previousTransform.localPosition.x;
        spawnPosition.y = transform.localPosition.y;
        fallingPlatform.transform.localPosition = spawnPosition;
        fallingPlatform.gameObject.SetActive(true);
        SpawnParticles(spawnPosition);
    }

    private void SpawnParticles(Vector2 spawnPos)
    {
        outlineParticles = particlesPool.Get();
        outlineParticles.transform.localPosition = spawnPos;
        outlineParticles.gameObject.SetActive(true);
    }
}
