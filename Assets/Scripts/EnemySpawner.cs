using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public float waitTime;

    private ObjectPool pool;

	// Use this for initialization
	void Start ()
    {
        pool = GetComponent<ObjectPool>();
        StartCoroutine(EnemySpawnCoroutine(waitTime));
	}

    private IEnumerator EnemySpawnCoroutine(float waitTime)
    {
        // wait for one tick to ensure the ObjectPool is populated
        yield return new WaitForFixedUpdate();

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
            obj.SetActive(true);

            yield return new WaitForSeconds(waitTime);
        }
    }
}
