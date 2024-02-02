using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    Camera cam; 

    public float cameraSensitivityX;
    public float cameraSensitivityY;

    public Transform cameraHolder;


    public float cameraRotationX;
    public float cameraRotationY;


    //i will remove


    //we need to check here for any other cameras and then we disable it.

    private void Awake()
    {
        cam = Camera.main;
    }

    public void MakeCamWatchFallDeath()
    {
        //
        //make it have no parent.
        cam.transform.parent = null;
        cam.transform.position += new Vector3(0, 5, 0);
        cam.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    public void ResetCam()
    {
        cam.transform.parent = cameraHolder;
        cam.transform.localPosition = Vector3.zero;
        cam.transform.rotation = Quaternion.Euler(0,0, 0);
    }
   
    public void SetRotationX(float newValueX)
    {
        cameraRotationX = newValueX;    

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


    //

    //the joystick is going to move this value.
}
