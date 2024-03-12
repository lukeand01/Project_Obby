using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeetCollider : MonoBehaviour
{

    //this script is so that we can be more precise with isgrounded.


    public bool IsGrounded { get; private set; }


    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == 6)
        {
            IsGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 6)
        {
            IsGrounded = false;
        }
        
    }




}
