using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour{ 

    private Transform dron;
    void Awake()
    {
        dron = GameObject.FindGameObjectWithTag("Player").transform;  
    }

    private Vector3 velocityCameraFollow;
    private Vector3 behindPosition = new Vector3(0, 1.5f, -4);
    public float angle;
    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, dron.transform.TransformPoint(behindPosition) + Vector3.up * 0.05f * Input.GetAxis("Vertical"), ref velocityCameraFollow, 0.1f);
        transform.rotation = Quaternion.Euler(new Vector3(angle, dron.GetComponent<DroneMovementScript>().rotacionYActual, 0));
    }
}


