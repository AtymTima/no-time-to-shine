using UnityEngine;

public class TorchSpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnerPoints;
    [SerializeField] float offsetY = 1;
    [SerializeField] float offsetX = 0.5f;
    [SerializeField] SoundManager soundManager;

    ObjectPool<CollectableTorch> torchPool;
    CollectableTorch collectableTorch;

    Vector2 spawnVector;
    int randomSpawnPoint;
    bool isLoaded;

    private void Awake()
    {
        torchPool = ObjectPool<CollectableTorch>.Instance;
        torchPool.AddObjects(1);
        CollectableTorch.onTorchCollected += SpawnTorch;
        CandleTimer.OnCandleTimerStop += RemoveAllTorches;
    }

    private void Start()
    {
        SpawnTorch(0);
    }

    private void OnDestroy()
    {
        CollectableTorch.onTorchCollected -= SpawnTorch;
        CandleTimer.OnCandleTimerStop -= RemoveAllTorches;
    }

    private void SpawnTorch(int previousIndex)
    {
        collectableTorch = torchPool.Get();
        GenerateRandomPoint(previousIndex);
        spawnVector.x = spawnerPoints[randomSpawnPoint].localPosition.x + offsetX;
        spawnVector.y = spawnerPoints[randomSpawnPoint].localPosition.y + offsetY;
        if (collectableTorch == null) { return; }
        collectableTorch.transform.localPosition = spawnVector;

        collectableTorch.SetParentPosition(spawnerPoints[randomSpawnPoint], randomSpawnPoint);
        collectableTorch.gameObject.SetActive(true);

        if (!isLoaded)
        {
            isLoaded = true;
            return;
        }
        soundManager.CandelTimerStartSFX(false);
    }

    private void GenerateRandomPoint(int previousIndex)
    {
        if (spawnerPoints.Length == 1)
        {
            randomSpawnPoint = 0;
            return;
        }
        do
        {
            randomSpawnPoint = Random.Range(0, spawnerPoints.Length);
        }
        while (previousIndex == randomSpawnPoint);
    }

    private void RemoveAllTorches()
    {
        torchPool.ReturnToPool(collectableTorch);
    }

    public CollectableTorch GetCurrentTorch()
    {
        return collectableTorch;
    }
}
