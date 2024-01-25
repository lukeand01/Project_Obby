using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchKill : MonoBehaviour
{
    //whanever this touches the player kill it.
    [SerializeField] bool notBlockable;
    float current;
    float total;

    private void Awake()
    {
        total = 0.15f;
    }

    private void FixedUpdate()
    {
        if(current > 0)
        {
            current -= 0.02f;
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        //we keep trying to kill.

        if (current > 0) return;

        if (collision.transform.tag != "Player") return;

        PlayerHandler.instance.TakeDamage(notBlockable);
        current = total;
    }

}
