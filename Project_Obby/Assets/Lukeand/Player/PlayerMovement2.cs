using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    //needs to be a better version.


    //important things.
    //the player has a walk durationToGetToPos



    PlayerHandler handler;

    [Separator("VITAL REFERENCES")]
    [Tooltip("This defines the direction the player takes")][SerializeField] Transform playerOrientation;

    [Separator("GROUND MOVEMENT SPEED")]
    [Tooltip("This is the speed at which it moves")]
    [SerializeField] float moveSpeed;
    [SerializeField] float accelerationRate;
    [SerializeField] float topAcceleration;
    [Tooltip("This is the highest speed this player can reach")]
    [SerializeField] float topSpeed;
    

    [Separator("JUMP VARIABLES")]
    [SerializeField] float jumpForce;
    [Tooltip("The time a player can still jump after no longer touching the ground.")][SerializeField] float coyoteTotal;
    [Tooltip("This timer allows for the player to press the button and still jump when the jump is allowed")][SerializeField] float bufferTotal;
    [SerializeField] float HoldJumpTotal;


    [Separator("JUMP POWERS")]
    [Tooltip("For the foguete power")][SerializeField] int JumpQuantityFromPower;
    [Tooltip("For the spring power")][SerializeField] int JumpForceFromPower;


    [Separator("COMPONENTS")]
    public PlayerFeetCollider feetCollider;



    float additionalJumpForceCurrent;
    float jumpQuantityCurrent;
    float jumpQuantityTotal;
    float coyoteCurrent;
    float holdJumpCurrent;
    float bufferCurrent;

    float accelerationIncrementCurrent;

    LayerMask groundMask;

    bool isGrounded;
    bool canJump;


    [SerializeField] Vector3 rbVelocity;

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

        rbVelocity = handler.rb.velocity;

        canJump = CanJump();

        ControlGroundSpeed();

        HandleGroundedLogic();
        HandleBufferLogic();

        //DebugErrorText.Log("isgrounded " + isGrounded);

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


        if (inputDir.y > 0.6)
        {
            accelerationIncrementCurrent += accelerationRate;
        }

        if(inputDir.y < -0.3)
        {
            accelerationIncrementCurrent = 0;
        }

        

        float clampedAccelerationSpeed = accelerationIncrementCurrent;
        clampedAccelerationSpeed = Mathf.Clamp(clampedAccelerationSpeed, 0, topAcceleration);

        
        float newSpeed = moveSpeed + clampedAccelerationSpeed;

        handler.rb.velocity = new Vector3(moveDir.x * newSpeed, handler.rb.velocity.y, moveDir.z * newSpeed);
    }
    public void StopPlayer()
    {
        accelerationIncrementCurrent = 0;
        handler.rb.velocity = new Vector3(0, handler.rb.velocity.y, 0);
    }

    public void CompleteStopPlayer()
    {

        accelerationIncrementCurrent = 0;
        handler.rb.velocity = new Vector3(0, 0, 0);
        

        //handler.rb.useGravity = false;
        //handler.rb.constraints = RigidbodyConstraints.FreezeAll;
        //Invoke(nameof(Restore), 1.5f);
        //handler.rb.constraints = RigidbodyConstraints.FreezePositionX;
        //handler.rb.constraints = RigidbodyConstraints.FreezePositionZ;
        //handler.rb.constraints = RigidbodyConstraints.FreezeRotation;
    }


    void Restore()
    {
        handler.rb.constraints = RigidbodyConstraints.None;
        handler.rb.constraints = RigidbodyConstraints.FreezePositionX;
        handler.rb.constraints = RigidbodyConstraints.FreezePositionZ;
        handler.rb.constraints = RigidbodyConstraints.FreezeRotation;
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
    bool canHold;
    bool canRelease { get; set; }

    public void PressJump()
    {


        if (!canJump)
        {
            bufferCurrent = bufferTotal;
            return;
        }

        isHoldingJump = true;
        canHold = true;
        canRelease = true;

        jumpQuantityCurrent += 1;

        float jumpModifier = 1;

        if(jumpQuantityCurrent > 1)
        {
            GameHandler.instance.soundHandler.CreateSFX(handler.sound.doubleJumpClip);
            jumpModifier = 0.6f;
        }
        else
        {
            GameHandler.instance.soundHandler.CreateSFX(handler.sound.jumpClip);
        }


        holdJumpCurrent = 0;

        float actualJumpForce = (jumpForce + additionalJumpForceCurrent) * jumpModifier;
        handler.rb.velocity = new Vector3(handler.rb.velocity.x, 0, handler.rb.velocity.z);
        handler.rb.AddForce(transform.up * actualJumpForce, ForceMode.Impulse);
    }
    public void HoldJump()
    {

        if (!canHold) return;

        holdJumpCurrent += tick;

        if (holdJumpCurrent > HoldJumpTotal)
        {
            //if its more it needs to start falling.
            StartFalling();
            return;
        }

        if (holdJumpCurrent > HoldJumpTotal * 0.7f)
        {
            //removes gravity.

            handler.rb.AddForce(transform.up * 0.08f, ForceMode.Impulse);
            return;
        }



    }
    public void ReleaseJump()
    {
        if (!canRelease)
        {
            return;
        }

        StartFalling();

              
    }

    void StartFalling()
    {
        if (handler.rb.velocity.y > 0)
        {
            handler.rb.velocity = new Vector3(0, handler.rb.velocity.y * 0.1f, 0);
        }

        canRelease = false;
        canHold = false;
        isHoldingJump = false;

    }


    void HandleGroundedLogic()
    {
        //whatever happens when its grounded or when its no longer grounded.
        if (isGrounded)
        {
            jumpQuantityCurrent = 0;
            coyoteCurrent = 0;

            if(bufferCurrent > 0)
            {
                bufferCurrent = 0;
                PressJump();
            }

        }
        else
        {
            HandleCoyote();
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
        if(coyoteTotal > coyoteCurrent)
        {
            coyoteCurrent += tick;
        }     

    }

    
    

    #endregion

    #region UTILS
    bool IsGrounded()
    {
        //float playerHeight = 0.55f;


        return feetCollider.IsGrounded;

        //Collider[] cast = Physics.OverlapSphere(feetCollider.transform.position, 0.25f, groundMask);
        //return cast.Length > 0;
    }

    bool CanJump()
    {
        //can jump if you have more 

        if(jumpQuantityCurrent >= jumpQuantityTotal)
        {
            return false;
        }


        if (isGrounded)
        {
            return true;
        }
        else
        {
            if (!isHoldingJump && coyoteCurrent < coyoteTotal)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        


    }


    string WhyJump()
    {
        if (jumpQuantityCurrent >= jumpQuantityTotal)
        {
            return "Failed because there are not enough jump quantitis";
        }


        if (isGrounded)
        {
            return "is grounded";
        }
        else
        {
            if (!isHoldingJump && coyoteCurrent < coyoteTotal)
            {
                return "Coyote";
            }
            else
            {
                return "Not coyote";
            }
        }
    }


    #endregion

    #region POWERS
    [Separator("DEBUG POWER")]
    [SerializeField] bool DEBUGHASDOUBLEJUMP;
    [SerializeField] bool DEBUGHASJUMPINCREMENT;

    public void AddDoubleJump()
    {
        jumpQuantityTotal = JumpQuantityFromPower;
        DEBUGHASDOUBLEJUMP = true;
    }
    public void RemoveDoubleJump()
    {
        jumpQuantityTotal = 1;
        DEBUGHASDOUBLEJUMP = false;
    }

    public void AddJumpIncrement()
    {
        additionalJumpForceCurrent = JumpForceFromPower;
        DEBUGHASJUMPINCREMENT = true;
    }
    public void RemoveJumpIncrement()
    {
        additionalJumpForceCurrent = 0;
        DEBUGHASJUMPINCREMENT = false;
    }

    #endregion
}
