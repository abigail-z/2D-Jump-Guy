using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public float waitTime;
    public ObjectPool pool;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(EnemySpawnCoroutine(waitTime));
	}

    private IEnumerator EnemySpawnCoroutine(float waitTime)
    {
        // wait for one tick to ensure the ObjectPool is populated
        yield return new WaitForFixedUpdate();

        while (true)
        {
            GameObject obj = pool.Pop();
            if (obj != null)
            {
                obj.transform.position = transform.position;
                obj.SetActive(true);
            }
#if UNITY_EDITOR
            else
            {
                Debug.Log("ObjectPool empty");
            }
#endif

            yield return new WaitForSeconds(waitTime);
        }
    }
}
