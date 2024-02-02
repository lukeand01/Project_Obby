using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchFallDeath : MonoBehaviour
{
    //if the player ever touches this it dies. but it falls for a bit more before dying.
    //the camera goes up and looks down, seeing the player fall and die. 
    //this blocks the player


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Player") return;
        PlayerHandler.instance.DieFromFall();
    }
    



}
