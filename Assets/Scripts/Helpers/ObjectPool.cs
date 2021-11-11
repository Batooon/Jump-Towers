using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private List<T> _pooledObjects = new List<T>();
    private readonly T _poolObject;
    private readonly int _poolAmount;

    public ObjectPool(T poolObject, int poolAmount)
    {
        _poolAmount = poolAmount;
        _poolObject = poolObject;
        SetupPool();
    }

    public T GetPooledObject()
    {
        for (var i = 0; i < _poolAmount; i++)
        {
            if (_pooledObjects[i].gameObject.activeInHierarchy == false)
                return _pooledObjects[i];
        }

        return null;
    }

    public T SpawnObjectAt(Vector3 position)
    {
        var objectToSpawn = GetPooledObject();
        objectToSpawn.transform.position = position;
        objectToSpawn.gameObject.SetActive(true);
        return objectToSpawn;
    }

    private void SetupPool()
    {
        T temp;
        for (var i = 0; i < _poolAmount; i++)
        {
            temp = Object.Instantiate(_poolObject);
            temp.gameObject.SetActive(false);
            _pooledObjects.Add(temp);
        }
    }
}
