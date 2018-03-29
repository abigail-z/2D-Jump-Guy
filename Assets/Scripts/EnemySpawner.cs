using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float initialWaitTime;
    public float initialEnemySpeed;
    public float enemySpeedIncreaseRate;
    public float spawnTimeDecreaseRate;

    public float wait;
    public float speed;
    private ObjectPool pool;

	// Use this for initialization
	void Start ()
    {
        wait = initialWaitTime;
        speed = initialEnemySpeed;
        pool = GetComponent<ObjectPool>();
        StartCoroutine(EnemySpawnCoroutine());
	}

    private IEnumerator EnemySpawnCoroutine()
    {
        while (true)
        {
            GameObject obj = pool.Pop();
            if (obj == null)
            {
#if UNITY_EDITOR
                Debug.Log("ObjectPool empty, creating new");
#endif
                obj = pool.Create();
            }

            obj.transform.position = transform.position;
            EnemyMovement movement = obj.GetComponent<EnemyMovement>();
            movement.moveSpeed = speed;
            obj.SetActive(true);

            yield return new WaitForSeconds(wait);
        }
    }

    public void IncreaseSpeed()
    {
        wait *= 1 - spawnTimeDecreaseRate;
        speed *= 1 + enemySpeedIncreaseRate;
    }
}
