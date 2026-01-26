using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private readonly T _prefab;
    private readonly Transform _parent;
    private readonly Queue<T> _pool = new Queue<T>();
    private readonly int _initialSize;

    public ObjectPool(T prefab, Transform parent = null, int initialSize = 10)
    {
        _prefab = prefab;
        _parent = parent;
        _initialSize = initialSize;

        for (int i = 0; i < _initialSize; i++)
        {
            T obj = CreateNewObject();
            _pool.Enqueue(obj);
        }
    }

    private T CreateNewObject()
    {
        T obj = Object.Instantiate(_prefab, _parent);
        obj.gameObject.SetActive(false);
        return obj;
    }

    public T Get()
    {
        T obj;

        if (_pool.Count > 0)
        {
            obj = _pool.Dequeue();
        }
        else
        {
            obj = CreateNewObject();
        }

        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }

    public void Clear()
    {
        while (_pool.Count > 0)
        {
            T obj = _pool.Dequeue();
            Object.Destroy(obj.gameObject);
        }
    }
}