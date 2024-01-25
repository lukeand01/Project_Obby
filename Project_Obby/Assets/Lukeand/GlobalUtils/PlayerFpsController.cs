using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFpsController
{
    Animator anim;


    public PlayerFpsController(Rigidbody rb, Transform orientation)
    {
        this.rb = rb;
        this.orientation = orientation;
    }

    #region MOVE

    Vector3 moveDirection;
    [SerializeField] float moveSpeed = 5;
    Rigidbody rb;
    Transform orientation;
    //but its better 


    public void MoveControl(Vector3 dir)
    {
        moveDirection = orientation.forward * dir.z + orientation.right * dir.x;
        rb.velocity = moveDirection * moveSpeed;
        Debug.Log("velocity " + rb.velocity);
    }

    #endregion


    #region MOUSE

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    [SerializeField][Range(0, 0.5f)] float mouseSmoothTime = 0f;
    [SerializeField] float mouseSensitivity = 3.5f;
    float cameraPitch = 0f;


    public void MouseControl(Transform player)
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        Camera.main.transform.localEulerAngles = Vector3.right * cameraPitch;

        player.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    #endregion
}
