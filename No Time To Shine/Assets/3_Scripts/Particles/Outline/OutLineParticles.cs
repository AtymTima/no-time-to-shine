using UnityEngine;

public class OutLineParticles : MonoBehaviour
{
    OutlineLifeTime objectFromPool;
    ObjectPool<OutlineLifeTime> objectPool;

    private bool startedRunning;

    private void Awake()
    {
        PlatformCollision.onTriggerCoroutine += SummonParticles;
    }

    private void Start()
    {
        objectPool = ObjectPool<OutlineLifeTime>.Instance;
        objectPool.AddObjects(3);
    }

    private void OnDestroy()
    {
        PlatformCollision.onTriggerCoroutine -= SummonParticles;
    }

    private void SummonParticles(float waitForSeconds, bool stopNow)
    {
        if (stopNow) { return; }
        objectFromPool = objectPool.Get();
        if (gameObject == null) { return; }
        objectFromPool.transform.localPosition = transform.position;
        objectFromPool.gameObject.SetActive(true);
    }
}
