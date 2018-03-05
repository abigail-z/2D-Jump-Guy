using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    public GameObject objectToPool;
    public uint maxCount;


    private Stack<GameObject> pool;

    private void Awake()
    {
        pool = new Stack<GameObject>();
    }

    // Use this for initialization
    void Start ()
    {
        for (uint i = 0; i < maxCount; ++i)
        {
            GameObject obj = Instantiate(objectToPool);
            obj.SetActive(false);
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
}
