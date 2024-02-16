using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{ 
    PlayerHandler handler;


    [Separator("MOVEMENT SOUNDS")]
    public AudioClip jumpClip;
    public AudioClip doubleJumpClip;

    [Separator("DEATH SOUNDS")]
    public AudioClip fallClip;
    public AudioClip deathClip;


}
