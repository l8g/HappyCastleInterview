using UnityEngine;
using System.Collections.Generic;

public class ObjectPool
{
    private Dictionary<string, Stack<GameObject>> pool;

    public ObjectPool()
    {
        pool = new Dictionary<string, Stack<GameObject>>();
    }

    public GameObject Get(string resourceName)
    {
        if (pool.ContainsKey(resourceName) && pool[resourceName].Count > 0)
        {
            GameObject obj = pool[resourceName].Pop();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            return LoadGameObject(resourceName);
        }
    }

    public void Release(GameObject obj, string resourceName)
    {
        if (!pool.ContainsKey(resourceName))
        {
            pool[resourceName] = new Stack<GameObject>();
        }
        obj.SetActive(false);
        pool[resourceName].Push(obj);
    }

    private GameObject LoadGameObject(string resourceName)
    {
        return Object.Instantiate(Resources.Load<GameObject>(resourceName));
    }
}
