using MyBox;
using UnityEngine;

public class PowerData : StoreData
{
    //each has a price
    //
    [Separator("POWER")]
    public string powerName;
    public Sprite powerSprite;


    public virtual void AddPower()
    {
        PlayerHandler.instance.AddPower(this);
    }
    public virtual void RemovePower()
    {

    }


    public override void Buy()
    {
        AddPower();
    }
    public override bool CanBuy()
    {
        //check if you have that power then we check 
        return true;

    }



}
