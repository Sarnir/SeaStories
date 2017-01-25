using System.Collections;
using System.Collections.Generic;

public class ObjectPool<T>
{
    List<T> pool;
    public ObjectPool()
    {
        pool = new List<T>();
    }
    /*
    public ShopItem GetElementFromPool()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].gameObject.activeSelf)
            {
                pool[i].gameObject.SetActive(true);
                return pool[i];
            }
        }

        ShopItem item;
        item = Instantiate(ShopItemPrefab, transform, false);
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
    }*/
}
