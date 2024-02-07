using DG.Tweening;
using GoogleMobileAds.Ump.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    Camera cam;

    public float cameraSensitivityX;
    public float cameraSensitivityY;

    public Transform cameraHolder;
    [SerializeField] Transform cameraHolderForDance;
    [SerializeField] Transform cameraHolderForIntroduction;

    public float cameraRotationX;
    public float cameraRotationY;
    public float playerRotationX;

    //i will remove


    //we need to check here for any other cameras and then we disable it.

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cam.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            cam.transform.localRotation = Quaternion.Euler(180, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            cam.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }



    }


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
        cam.transform.localRotation = Quaternion.Euler(90, 0, 0);
    }

    public IEnumerator RotateCameraForDanceProcess(float timer)
    {

        cam.transform.SetParent(cameraHolderForDance);
        cam.transform.DOLocalMove(Vector3.zero, timer);
        cam.transform.DOLocalRotate(new Vector3(0, 180, 0), timer);

        yield return new WaitForSeconds(timer);

    }

    //i am having a problem with controlling the camera. 

    public void ResetCam()
    {
        cam.transform.SetParent(cameraHolder);
        cam.transform.localPosition = Vector3.zero;
        cam.transform.localRotation = Quaternion.Euler(0,0, 0);
    }
    public void EspecialResetCam()
    {
        Debug.Log("especial rest was called");
        cam.transform.SetParent(cameraHolder);
        cam.transform.localPosition = Vector3.zero;
        //cam.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
   
    public void ResetCamToIntroduction()
    {
        cam.transform.SetParent(cameraHolderForIntroduction);
        cam.transform.localPosition = new Vector3(0, 1.5f, -0.5f);
        cam.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public IEnumerator CamIntroductionProcess()
    {
        yield return new WaitForSeconds(0.6f);

        float timer = 1.2f;

        cam.transform.DOMove(cameraHolder.position, timer);

        yield return new WaitForSeconds(timer);

        ResetCam();

    }


    public void SetRotationX(float newValueX)
    {
        //playerRotationX = newValueX;
        cameraRotationX = newValueX;
    }
  
    public void MoveCameraByJoystick(Vector3 dir)
    {
        dir = dir.normalized;
        cameraRotationX += dir.x * cameraSensitivityX;
        playerRotationX += dir.x * cameraSensitivityX;
        cameraRotationY -= dir.y * cameraSensitivityY;
        float clampedCamerRotationY = cameraRotationY;
        clampedCamerRotationY = Mathf.Clamp(clampedCamerRotationY, -30, 25);
        transform.rotation = Quaternion.Euler(0, cameraRotationX, 0);
        cameraHolder.rotation = Quaternion.Euler(clampedCamerRotationY, cameraRotationX, 0);

        
        
    }


    //

    //the joystick is going to move this value.
}
