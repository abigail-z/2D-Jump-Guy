using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject objectToFollow;
    public float maxFollowTime;
    public float maxFollowSpeed;

    private Vector3 origin;
    private Vector3 cameraPos;
    private Vector3 refVelocity;

    // Use this for initialization
    void Start ()
    {
        origin = transform.position;
        cameraPos = origin + objectToFollow.transform.position / 2;
        cameraPos.z = -1;
        transform.position = cameraPos;
        refVelocity = Vector3.zero;
	}

    void LateUpdate()
    {
        if (objectToFollow.activeInHierarchy)
            cameraPos = origin + objectToFollow.transform.position / 2;
        else
            cameraPos = origin;
        transform.position = Vector3.SmoothDamp(transform.position, cameraPos, ref refVelocity, maxFollowTime, maxFollowSpeed, Time.deltaTime);
    }
}
