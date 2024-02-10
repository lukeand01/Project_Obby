using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerMovement2 : MonoBehaviour
{
    //needs to be a better version.


    //important things.
    //the player has a walk speed



    PlayerHandler handler;

    [Separator("VITAL REFERENCES")]
    [Tooltip("This defines the direction the player takes")][SerializeField] Transform playerOrientation;

    [Separator("GROUND MOVEMENT SPEED")]
    [Tooltip("This is the speed at which it moves")][SerializeField] float moveSpeed;
    [Tooltip("This is the highest speed this player can reach")][SerializeField] float topSpeed;

    [Separator("JUMP VARIABLES")]
    [SerializeField] float jumpForce;
    [Tooltip("The time a player can still jump after no longer touching the ground.")][SerializeField] float coyoteTotal;
    [Tooltip("This timer allows for the player to press the button and still jump when the jump is allowed")][SerializeField] float bufferTotal;
    [SerializeField] float HoldJumpTotal;

    [Separator("FALLS SPEED")]
    [SerializeField] float fallSpeedWhileHolding;
    [SerializeField] float fallSpeedWhileNotHolding;

    [Separator("JUMP POWERS")]
    [Tooltip("For the foguete power")][SerializeField] int JumpQuantityFromPower;
    [Tooltip("For the spring power")][SerializeField] int JumpForceFromPower;

    float additionalJumpForceCurrent;
    float jumpQuantityCurrent;
    float jumpQuantityTotal;
    float coyoteCurrent;
    float holdJumpCurrent;
    float bufferCurrent;

    LayerMask groundMask;

    bool isGrounded;
    bool canJump;


    float tick = 0.02f;

    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();

        groundMask = (1 << 6);

        jumpQuantityTotal = 1;
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        canJump = CanJump();

        ControlGroundSpeed();
        ControlFallSpeed();

        HandleCoyote();
        HandleGroundedLogic();
        HandleBufferLogic();

        
        CallCommandForJumpIfPossible();

    }

    public void ResetMovement()
    {
        isHoldingJump = false;
    }


    #region MOVEMENT

    public void MovePlayer(Vector3 inputDir)
    {
        Vector3 moveDir = playerOrientation.forward * inputDir.y + playerOrientation.right * inputDir.x;
        //handler.rb.AddForce(moveDir.normalized * moveSpeed, ForceMode.Force);
        handler.rb.velocity = new Vector3(moveDir.x * moveSpeed, handler.rb.velocity.y, moveDir.z * moveSpeed);
    }
    public void StopPlayer()
    {
        handler.rb.velocity = new Vector3(0, handler.rb.velocity.y, 0);
    }

    //the fall is not controlled here
    void ControlGroundSpeed()
    {
        Vector3 flatVel = new Vector3(handler.rb.velocity.x, 0, handler.rb.velocity.z);

        if (flatVel.magnitude > topSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * topSpeed;
            handler.rb.velocity = new Vector3(limitedVel.x, handler.rb.velocity.y, limitedVel.z);
        }
    }

    #endregion

    #region JUMP
    bool isHoldingJump;

    public void PressJump()
    {
        isHoldingJump = true;

        jumpQuantityCurrent += 1;

        float jumpModifier = 1;

        if(jumpQuantityCurrent > 1)
        {
            jumpModifier = 0.6f;
        }


        float actualJumpForce = (jumpForce + additionalJumpForceCurrent) * jumpModifier;
        handler.rb.velocity = new Vector3(handler.rb.velocity.x, 0, handler.rb.velocity.z);
        handler.rb.AddForce(transform.up * actualJumpForce, ForceMode.Impulse);
    }
    public void HoldJump()
    {

    }
    public void ReleaseJump()
    {
        isHoldingJump = false;




    }

    void ControlFallSpeed()
    {
        //if i am holding its one thing if i am not its another.

        if (isHoldingJump)
        {

        }
        else
        {

        }


    }


    void HandleGroundedLogic()
    {
        //whatever happens when its grounded or when its no longer grounded.
        if (isGrounded)
        {


        }
        else
        {

        }



    }

    void HandleBufferLogic()
    {
        if(bufferCurrent > 0)
        {
            bufferCurrent -= tick;
        }
    }
   
    void HandleCoyote()
    {
        if (coyoteCurrent >= coyoteTotal)
        {
            //coyote is turned off.

        }
        else
        {
            coyoteCurrent += tick;
        }

    }

    
    void CallCommandForJumpIfPossible()
    {
        if(isGrounded && bufferCurrent > 0)
        {
            //then we jump.
            


        }
    }

    #endregion

    #region UTILS
    bool IsGrounded()
    {
        float playerHeight = 2.5f;
        return Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);
    }

    bool IsCloseEnoughToGroundForBuffer()
    {
        float playerHeight = 2.5f;
        return Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 1.2f, groundMask);
    }


    bool CanJump()
    {
        //can jump if you have more 

        if(jumpQuantityTotal >= jumpQuantityCurrent)
        {
            return false;
        }


        if (isGrounded)
        {
            return true;
        }
        else
        {
            if (coyoteTotal >= coyoteCurrent)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        


    }

    #endregion

    #region POWERS
    public void AddDoubleJump()
    {
        jumpQuantityTotal = JumpQuantityFromPower;
    }
    public void RemoveDoubleJump()
    {
        jumpQuantityTotal = 1;
    }

    public void AddJumpIncrement()
    {
        additionalJumpForceCurrent = JumpForceFromPower;
    }
    public void RemoveJumpIncrement()
    {
        additionalJumpForceCurrent = 0;
    }

    #endregion
}
