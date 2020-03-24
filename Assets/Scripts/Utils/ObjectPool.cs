using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    List<T> pool;
    T prefab;
    Transform parent;
    public ObjectPool(T _prefab, Transform _parent)
    {
        pool = new List<T>();
        prefab = _prefab;
        parent = _parent;
    }
    
    public T GetElementFromPool()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].gameObject.activeSelf)
            {
                pool[i].gameObject.SetActive(true);
                return pool[i];
            }
        }

        T item;
        item = Object.Instantiate(prefab, parent, false);
        item.gameObject.SetActive(true);
        pool.Add(item);

        return item;
    }

    public void SetAllElementsInactive()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            pool[i].gameObject.SetActive(false);
        }
    }

    public List<T> GetAllActiveElements()
    {
        List<T> elements = new List<T>();

        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].gameObject.activeSelf)
                elements.Add(pool[i]);
        }

        return elements;
    }
}
