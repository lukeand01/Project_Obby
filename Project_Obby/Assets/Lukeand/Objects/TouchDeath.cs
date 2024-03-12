using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDeath : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        //kills the player if the player has no shield.;

        PlayerHandler.instance.TakeDamage(false);
    }
}
