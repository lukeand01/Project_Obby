using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //is this being used?

    PlayerHandler handler;

    [Separator("STUFF FOR DEBUGGING")]
    [SerializeField] bool DEBUGCanMoveWithASWDKeys;

    [SerializeField] Joystick joystickMove;
    [SerializeField] Joystick joystickCamera;
    InputButton inputButtonJump;

    public BlockClass blockClass {  get; private set; } 

    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();
    }

    private void Start()
    {

        blockClass = new BlockClass();

        joystickMove = UIHandler.instance.moveJoystick;
        joystickCamera = UIHandler.instance.cameraJoystick;
        inputButtonJump = UIHandler.instance.inputButtonJump;

        inputButtonJump.EventPressed += handler.movement2.PressJump;
        inputButtonJump.EventReleased += handler.movement2.ReleaseJump;
    }

    private void OnDisable()
    {
        if(handler.movement2 != null)
        {
            inputButtonJump.EventPressed -= handler.movement2.PressJump;
            inputButtonJump.EventReleased -= handler.movement2.ReleaseJump;
        }

        
    }


    private void Update()
    {

        if (blockClass.HasBlock(BlockClass.BlockType.Complete)) return;
        ControlJumpInput();
        DebugJumpWithKeys();
    }
    private void FixedUpdate()
    {
        if (blockClass.HasBlock(BlockClass.BlockType.Complete))
        {
            //ControlCameraWithJoystick(true);
            return;
        }

        if (DEBUGCanMoveWithASWDKeys)
        {
            DEBUGMovementWithKeys();
        }
        else
        {
            ControlCameraWithJoystick();
            ControlMovementWithJoystick();
        }
    }


    void DEBUGMovementWithKeys()
    {
        Vector3 dir = Vector3.zero;


        if (Input.GetKey(KeyCode.W))
        {

        }
        if (Input.GetKey(KeyCode.A))
        {

        }
        if (Input.GetKey(KeyCode.D))
        {

        }
        if (Input.GetKey(KeyCode.S))
        {

        }



    }

    void DebugJumpWithKeys()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            handler.movement2.PressJump();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            handler.movement2.HoldJump();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            handler.movement2.ReleaseJump();
        }
    }

    void ControlMovementWithJoystick()
    {
        //bascially
        //if you move to the right it stands still and looks to the right. rotation is quite fast.
        //if you move forward or backward it moves. 

        if(joystickMove.Direction != Vector2.zero)
        {
            handler.movement2.MovePlayer(joystickMove.Direction);
            //if there is a confirmation menu we stop it.

        }
        else
        {
            handler.movement2.StopPlayer();
        }

        

    }
    void ControlCameraWithJoystick()
    {
        //this moves the camera.
       
        handler.cam.MoveCameraByJoystick(joystickCamera.Direction);
    }

    void ControlJumpInput()
    {
        if(inputButtonJump.value == 1)
        {
            handler.movement2.HoldJump();
        }
    }
}
