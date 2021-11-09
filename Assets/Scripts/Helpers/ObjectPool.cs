using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private List<GameObject> _pooledObjects = new List<GameObject>();
    private readonly GameObject _poolObject;
    private readonly int _poolAmount;

    public ObjectPool(GameObject poolObject, int poolAmount)
    {
        _poolAmount = poolAmount;
        _poolObject = poolObject;
        SetupPool();
    }

    public GameObject GetPooledObject()
    {
        for (var i = 0; i < _poolAmount; i++)
        {
            if (_pooledObjects[i].activeInHierarchy == false)
                return _pooledObjects[i];
        }

        return null;
    }

    public GameObject SpawnObjectAt(Vector3 position)
    {
        var objectToSpawn = GetPooledObject();
        objectToSpawn.transform.position = position;
        objectToSpawn.SetActive(true);
        return objectToSpawn;
    }

    private void SetupPool()
    {
        GameObject temp;
        for (var i = 0; i < _poolAmount; i++)
        {
            temp = Object.Instantiate(_poolObject);
            temp.SetActive(false);
            _pooledObjects.Add(temp);
        }
    }
}
