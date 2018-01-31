using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float cameraSpeed;

    private Vector3 startPos;
	// Use this for initialization
	void Start ()
    {
        startPos = transform.position;
        transform.position = startPos + (player.transform.position / 2);
	}

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, startPos + (player.transform.position / 2),
            Time.deltaTime * cameraSpeed);
    }
}
