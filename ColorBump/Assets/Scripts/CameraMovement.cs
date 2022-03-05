using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float cameraSpeed = 0.8f;
    public Vector3 camVelocity;

    // Update is called once per frame
    void Update()
    {
        if(FindObjectOfType<PlayerController>().canMove)
             transform.position += Vector3.forward * cameraSpeed ;
        camVelocity= Vector3.forward * cameraSpeed ;
    }
}
