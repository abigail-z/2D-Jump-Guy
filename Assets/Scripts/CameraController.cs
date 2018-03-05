using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject objectToFollow;
    public float maxFollowTime;
    public float maxFollowSpeed;

    private Vector3 refVelocity;

	// Use this for initialization
	void Start ()
    {
        Vector3 playerPos = objectToFollow.transform.position;
        playerPos.z = -1;
        transform.position = playerPos;
        refVelocity = Vector3.zero;
	}

    void LateUpdate()
    {
        Vector3 playerPos = objectToFollow.transform.position;
        playerPos.z = -1;
        transform.position = Vector3.SmoothDamp(transform.position, playerPos, ref refVelocity, maxFollowTime, maxFollowSpeed, Time.deltaTime);
    }
}
