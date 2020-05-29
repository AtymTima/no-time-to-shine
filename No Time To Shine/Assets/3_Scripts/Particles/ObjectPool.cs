using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : Component
{
    public static ObjectPool<T> Instance { get; private set;}

    [SerializeField] private T objectPrefab;
    private Queue<T> objectsOfPool= new Queue<T>();

    T newObject;

    private void Awake()
    {
        Instance = this;
    }

    public T Get()
    {
        if (objectsOfPool.Count == 0)
        {
            AddObjects(1);
        }
        return objectsOfPool.Dequeue();
    }

    public void AddObjects(int objectCount)
    {
        for (int i=0; i < objectCount; i++)
        {
            newObject = Instantiate(objectPrefab);
            newObject.gameObject.SetActive(false);
            objectsOfPool.Enqueue(newObject);
        }
    }

    public void ReturnToPool(T returnedObject)
    {
        returnedObject.gameObject.SetActive(false);
        objectsOfPool.Enqueue(returnedObject);
    }
}
