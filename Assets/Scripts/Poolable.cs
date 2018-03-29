using UnityEngine;

public class Poolable : MonoBehaviour
{
    public ObjectPool Pool { set { pool = value; } }
    private ObjectPool pool;

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
        pool.Push(gameObject);
    }
}
