using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    public GameObject objectToPool;
    public int maxEnemyCount;
    public static ObjectPooler SharedInstance;

    private Stack<GameObject> pool;

    private void Awake()
    {
        SharedInstance = this;
        pool = new Stack<GameObject>();
    }

    // Use this for initialization
    void Start ()
    {
        pool.Push(objectToPool);
        for (uint i = 0; i < maxEnemyCount; ++i)
        {
            GameObject obj = Instantiate(objectToPool);
            obj.SetActive(false);
            pool.Push(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        if (pool.Count > 0)
        {
            return pool.Pop();
        }

        return null;
    }

    public void PoolObject(GameObject obj)
    {
        pool.Push(obj);
    }
}
