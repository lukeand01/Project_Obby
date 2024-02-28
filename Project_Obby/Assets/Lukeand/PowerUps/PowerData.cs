using MyBox;
using UnityEngine;

public class PowerData : ScriptableObject
{
    //each has a price
    //
    [Separator("POWER")]
    public string powerName;
    public Sprite powerSprite;

    [TextArea]public string temporaryPowerDescription;
    public int temporaryPowerPrice; //always gold.


    public virtual void AddPower()
    {

    }
    public virtual void RemovePower()
    {

    }



}
