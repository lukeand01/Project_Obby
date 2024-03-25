using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDeath : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        //kills the player if the player has no shield.;

        if(other.gameObject.layer == 8)
        {
            Debug.Log("this was the feet");
            return;
        }

        if (other.gameObject.layer != 3) return;

        PlayerHandler.instance.TakeDamage(false);
    }
}
