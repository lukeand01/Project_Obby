using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchVictory : MonoBehaviour
{

    [SerializeField] AudioClip audioClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;

        //UIHandler.instance.uiEnd.StartVictory();

        GameHandler.instance.soundHandler.CreateSFX(audioClip);
        PlayerHandler.instance.PlayerWon();

    }
}
