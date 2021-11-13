using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Object = UnityEngine.Object;

public class ObjectPool<T> where T : Platform
{
    private readonly List<T> _pooledObjects = new List<T>();
    private readonly T _poolObject;
    private readonly int _poolAmount;

    public ObjectPool(T poolObject, int poolAmount)
    {
        _poolAmount = poolAmount;
        _poolObject = poolObject;
        //SetupPool();
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

    public T ShowObjectAt(T objectToShow, Vector3 position)
    {
        objectToShow.transform.position = position;
        objectToShow.gameObject.SetActive(true);
        return objectToShow;
    }

    public T AddObject(T newObject)
    {
        T temp;
        temp = Object.Instantiate(newObject);
        temp.gameObject.SetActive(false);
        _pooledObjects.Add(temp);
        return temp;
    }

    public T GetObject(T searchObject)
    {
        foreach (var obj in _pooledObjects)
        {
            if(obj.gameObject.activeInHierarchy)
                continue;
            if (obj.GetType() == searchObject.GetType())
                return obj;
        }

        return null;
    }

    public void SetupPool()
    {
        for (var i = 0; i < _poolAmount; i++)
        {
            AddObject(_poolObject);
        }
    }
}
