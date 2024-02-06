using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceData : StoreData
{
    //this carries some kind of informating regarding the dance system.
    //we read this to decide what animation we will be using.


    public string animationName;
    public int animationId;

    public override void Buy()
    {
        //give the dance to the player. the player also has to choose what it will be using.

    }

    public override bool CanBuy()
    {
        //if you have the dance.
        return true;
    }
}
