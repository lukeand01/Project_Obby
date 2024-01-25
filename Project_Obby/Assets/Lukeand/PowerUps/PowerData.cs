using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerData : ScriptableObject
{
    //each has a price
    //
    public string powerName;
    public Sprite powerSprite;
    public int powerBaseCost;


    public virtual void AddPower()
    {
        PlayerHandler.instance.AddPower(this);
    }
    public abstract void RemovePower();
    
}
