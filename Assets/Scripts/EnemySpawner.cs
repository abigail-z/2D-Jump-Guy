using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public float waitTime;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(EnemySpawnCoroutine(waitTime));
	}

    private IEnumerator EnemySpawnCoroutine(float waitTime)
    {
        while (true)
        {
            GameObject obj = ObjectPooler.SharedInstance.GetPooledObject();
            if (obj != null)
            {
                obj.transform.position = transform.position;
                obj.SetActive(true);
            }

            yield return new WaitForSeconds(waitTime);
        }
    }
}
