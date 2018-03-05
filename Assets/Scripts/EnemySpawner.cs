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
        while (true)
        {
            GameObject obj = pool.Pop();
            if (obj != null)
            {
                obj.transform.position = transform.position;
                obj.SetActive(true);
            }

            yield return new WaitForSeconds(waitTime);
        }
    }
}
