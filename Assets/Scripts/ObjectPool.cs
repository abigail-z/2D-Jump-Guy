using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    public GameObject objectToPool;
    public uint maxCount;

    private Stack<GameObject> pool;

    private void Awake()
    {
        pool = new Stack<GameObject>();

        for (uint i = 0; i < maxCount; ++i)
        {
            GameObject obj = Instantiate(objectToPool);
            obj.SetActive(false);

            Poolable script = obj.GetComponent<Poolable>();
            script.SetPool(this);

            pool.Push(obj);
        }
    }

    public GameObject Pop()
    {
        if (pool.Count > 0)
        {
            return pool.Pop();
        }

        return null;
    }

    public void Push(GameObject obj)
    {
        pool.Push(obj);
    }

    public GameObject Create()
    {
        GameObject obj = Instantiate(objectToPool);
        obj.SetActive(false);

        Poolable script = obj.GetComponent<Poolable>();
        script.SetPool(this);

        return obj;
    }
}

