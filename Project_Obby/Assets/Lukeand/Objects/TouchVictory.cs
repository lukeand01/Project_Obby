using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchVictory : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;

        //UIHandler.instance.uiEnd.StartVictory();
        PlayerHandler.instance.PlayerWon();

    }
}
