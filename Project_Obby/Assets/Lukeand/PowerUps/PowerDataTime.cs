using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Power / Timer")]
public class PowerDataTime : PowerData
{
    //time to controll 


    public override void AddPower()
    {
        base.AddPower();
        PlayerHandler.instance.AddTimerModifier();
    }

    public override void RemovePower()
    {
        base.RemovePower();
        PlayerHandler.instance.RemoveTimerModifier();
    }
}
