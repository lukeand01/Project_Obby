using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTillEnd : MonoBehaviour
{

    //it moves till it gets to positions then it disappears.
    //we have to synch. only happens when the thing is called.

    Transform target;
    protected float speed;
    protected bool cantFollow;
    protected SpriteRenderer rend;

    public void SetUpBase(Transform target, float speed)
    {
        this.target = target;
        this.speed = speed;
        rend = GetComponent<SpriteRenderer>();
    }

    

    private void FixedUpdate()
    {
        if (target == null) return;
        if (cantFollow) return;




        if(transform.position != target.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed);
        }
        else
        {
            Act();
        }
    }



    protected virtual void Act()
    {
        Destroy(gameObject);
    }




}
