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

            Poolable poolable = obj.GetComponent<Poolable>();
            poolable.Pool = this;

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

        Poolable poolable = obj.GetComponent<Poolable>();
        // unnecessary?
        // it's kind of dumb to require objects to extend poolable only implicitly
        // but also i am dumb so it's fair
        /*
        if (poolable != null)
        {
            poolable.Pool = this;
        }
        else
        {
            obj.AddComponent<Poolable>();
            poolable = obj.GetComponent<Poolable>();
            poolable.Pool = this;
        }
        */
        poolable.Pool = this;
        

        return obj;
    }
}
