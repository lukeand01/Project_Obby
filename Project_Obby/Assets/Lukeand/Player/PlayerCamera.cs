using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float cameraSensitivityX;
    public float cameraSensitivityY;

    public Transform cameraHolder;


    public float cameraRotationX;
    public float cameraRotationY;

    //we need to check here for any other cameras and then we disable it.

    private void Start()
    {
        

    }

   

    public void MoveCameraByJoystick(Vector3 dir)
    {
        dir = dir.normalized;

        cameraRotationX += dir.x * cameraSensitivityX;
        cameraRotationY -= dir.y * cameraSensitivityY;
        float clampedCamerRotationY = cameraRotationY;
        clampedCamerRotationY = Mathf.Clamp(clampedCamerRotationY, -30, 25);
        transform.rotation = Quaternion.Euler(0, cameraRotationX, 0);
        cameraHolder.rotation = Quaternion.Euler(clampedCamerRotationY, cameraRotationX, 0);

        
        
    }


    //the joystick is going to move this value.
}
