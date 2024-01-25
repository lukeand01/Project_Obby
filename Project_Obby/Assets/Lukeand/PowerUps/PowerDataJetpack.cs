using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Power / Jetpack")]
public class PowerDataJetpack : PowerData
{


    public override void AddPower()
    {
        base.AddPower();
        PlayerHandler.instance.movement.AddDoubleJump();
    }

    public override void RemovePower()
    {
        PlayerHandler.instance.movement.RemoveDoubleJump();
    }
}
