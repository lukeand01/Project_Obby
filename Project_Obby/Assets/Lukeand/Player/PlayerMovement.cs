using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //this is going to handle all of the playermovement.
    //the jump needs to be mored tamed.




    [Separator("STAT")]
    [SerializeField] float baseSpeed;
    [SerializeField] float topSpeed;
    [SerializeField] float DEBUGXVELOCITY;
    [SerializeField] float dragSpeed;

    float currentSpeed;

    [SerializeField] Transform orientation;

    Vector3 moveDir;
    Vector3 lastDir;

    PlayerHandler handler;

    
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] bool isGrounded;
    float groundDrag;

    [Separator("Jump")]
    [SerializeField] float jumpModifier;
    [SerializeField] float jumpCooldown;
    [SerializeField] float totalCoyote;
    [SerializeField] float maxFallSpeed;

    float currentJumpCooldown;

    float currentCoyote;
    bool canJump;
    bool hasAlreadyReleasedJump;
    bool hasPressedJump;

    int totalJumpAmountAllowed;
    int currentJumpAmount;

    float jumpIncrement;


    [Separator("COMPONENTS")]
    [SerializeField] BoxCollider feetCollider;


    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();
        totalJumpAmountAllowed = 1;

        currentSpeed = baseSpeed;
    }


    void DEBUGMOVEMENT()
    {
        DEBUGXVELOCITY = handler.rb.velocity.y;
    }

    private void Update()
    {


        CalculateGroundedLogic();
        canJump = CanJump();

        SpeedControl();
        //DEBUGMOVEMENT();
        HandleJumpCooldown();      
        ControlRBDrag();

        if (isGrounded)
        {
            currentCoyote = 0;
            currentJumpAmount = 0;
            hasAlreadyReleasedJump = false;
        }
        else
        {
            HandleCoyote();
        }

       
        ControlJumpFallSpeed();
    }



    void HandleCoyote()
    {
        if(currentCoyote >= totalCoyote)
        {
            //coyote is turned off.

        }
        else
        {
            currentCoyote += Time.deltaTime;
        }

    }


    #region MOVEMENT
    public void MovePlayer(Vector3 inputDir)
    {
        moveDir = orientation.forward * inputDir.y + orientation.right * inputDir.x;
        handler.rb.AddForce(moveDir.normalized * currentSpeed, ForceMode.Force);
    }

    public void StopPlayer()
    {
        handler.rb.velocity = new Vector3(0, handler.rb.velocity.y, 0);
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(handler.rb.velocity.x, 0, handler.rb.velocity.z);

        if(flatVel.magnitude > topSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * topSpeed;
            handler.rb.velocity = new Vector3(limitedVel.x, handler.rb.velocity.y, limitedVel.z);
        }
        

    }

    #endregion

    #region JUMP

    void ControlRBDrag()
    {
        if (isGrounded)
        {
            handler.rb.drag = dragSpeed;
        }
        else
        {
            handler.rb.drag = 0;
        }
    }

    void HandleJumpCooldown()
    {
        if (currentJumpCooldown > 0)
        {
            currentJumpCooldown -= Time.deltaTime;
        }
    }

    void CalculateGroundedLogic()
    {

        //we need to check with a collider instea of a trace

       isGrounded = Physics.Raycast(feetCollider.bounds.center, Vector3.down, feetCollider.bounds.extents.y);

        Debug.Log("is grounded " + isGrounded);

        return;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
    }

    bool CanJump()
    {


        if (currentJumpCooldown > 0 && totalJumpAmountAllowed != 2)
        {
            return false;
        }

        if (currentJumpAmount == 1 && totalJumpAmountAllowed == 2)
        {
            return true;
        }

        if (isGrounded)
        {
            return true;
        }
        else
        {
            if (!hasPressedJump)
            {
                if (currentCoyote >= totalCoyote)
                    return false;
                else
                {

                    return true;
                }

            }
            else
            {
                return false;
            }


        }
    }

    public void StartJumpPlayer()
    {
        if (handler.controller.blockClass.HasBlock(BlockClass.BlockType.Complete)) return;


        if (!canJump)
        {
            return;
        }

        float secondJumpModifier = 1;

        if(currentJumpAmount == 1)
        {
            secondJumpModifier = 0.5f;
            return;
        }


        hasAlreadyReleasedJump = false;
        hasPressedJump = true;
        currentJumpCooldown = jumpCooldown;
        currentJumpAmount += 1;
        handler.rb.velocity = new Vector3(handler.rb.velocity.x, 0, handler.rb.velocity.z);
        handler.rb.AddForce(transform.up * (jumpModifier + jumpIncrement) * secondJumpModifier, ForceMode.Impulse);

    }
    public void HoldJumpPlayer()
    {
        if (handler.controller.blockClass.HasBlock(BlockClass.BlockType.Complete)) return;

        //go a little bit higher.
        //handler.rb.AddForce(transform.up * (jumpModifier * Time.deltaTime), ForceMode.Impulse);
    }

    public void ReleaseJumpPlayer()
    {
        //define differente speeds to fall.
        if (handler.controller.blockClass.HasBlock(BlockClass.BlockType.Complete)) return;

        float secondJumpModifier = 1;

        if(currentJumpAmount == 2)
        {
            secondJumpModifier = 1.5f;
        }

        if (hasAlreadyReleasedJump) return;

        hasAlreadyReleasedJump = true;
        handler.rb.velocity -= new Vector3(0, 4 * secondJumpModifier, 0);
        hasPressedJump = false;
    }
    void ControlJumpFallSpeed()
    {
        if(handler.rb.velocity.y < -2)
        {
            
            float clampedFall = handler.rb.velocity.y;
            clampedFall = Mathf.Clamp(clampedFall, -maxFallSpeed, 2);
            handler.rb.velocity = new Vector3(handler.rb.velocity.x, clampedFall, handler.rb.velocity.z);

        }
    }
    #endregion

    #region MOVEMENT POWER UPS

    [SerializeField] bool DEBUGDOUBLEJUMP;

    public void AddDoubleJump()
    {
        totalJumpAmountAllowed = 2;

        DEBUGDOUBLEJUMP = true;
    }
    public void RemoveDoubleJump()
    {
        totalJumpAmountAllowed = 1;
    }

    public void AddJumpIncrement()
    {
        jumpIncrement = 5;

        DEBUGDOUBLEJUMP = true;
    }
    public void RemoveJumpIncrement()
    {
        jumpIncrement = 0;
    }

    #endregion
}


//clamped fall speed
//after some time it falls faster.