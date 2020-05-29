using UnityEngine;

public class CollectableTorch : MonoBehaviour
{
    public delegate void OnTorchCollected(int previousIndex);
    public static event OnTorchCollected onTorchCollected = delegate { };

    [SerializeField] float offsetY = 1;
    [SerializeField] float offsetX = 0.5f;

    ObjectPool<CollectableTorch> torchPool;
    Transform parentPosition;
    Vector2 currentPosition;
    int currentIndex;

    private void Awake()
    {
        torchPool = ObjectPool<CollectableTorch>.Instance;
    }

    public void SetParentPosition(Transform parentObject, int parentIndex)
    {
        parentPosition = parentObject;
        currentIndex = parentIndex;
        currentPosition.y = parentPosition.localPosition.y + offsetY;
    }

    private void Update()
    {
        currentPosition.x = parentPosition.position.x + offsetX;
        gameObject.transform.localPosition = currentPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            torchPool.ReturnToPool(this);
            onTorchCollected?.Invoke(currentIndex);
        }
    }
}
