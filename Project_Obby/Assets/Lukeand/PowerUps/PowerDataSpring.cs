using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Power / Spring")]
public class PowerDataSpring : PowerData
{

    public override void AddPower()
    {
        base.AddPower();
        PlayerHandler.instance.movement.AddJumpIncrement();
    }
    public override void RemovePower()
    {
        PlayerHandler.instance.movement.RemoveJumpIncrement();
    }
}
