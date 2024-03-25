using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchJumper : MonoBehaviour
{
    [SerializeField] float jumpForce = 100f;

    private void OnCollisionEnter(Collision collision)
    {
        //send the player flying up.
        //
        if (collision.transform.tag != "Player") return;

        PlayerHandler.instance.movement2.JumperJump(jumpForce, 1);


    }


}
