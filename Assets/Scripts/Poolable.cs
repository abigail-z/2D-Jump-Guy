using UnityEngine;
public class Poolable : MonoBehaviour
{
    public ObjectPool pool;
    public void SetPool(ObjectPool pool)
    {
        this.pool = pool;
    }

    public void AddToPool()
    {
        gameObject.SetActive(false);
        pool.Push(gameObject);
    }
}
