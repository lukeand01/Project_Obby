using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Power / Jetpack")]
public class PowerDataJetpack : PowerData
{


    public override void AddPower()
    {
        base.AddPower();
        PlayerHandler.instance.movement2.AddDoubleJump();
    }

    public override void RemovePower()
    {
        PlayerHandler.instance.movement2.RemoveDoubleJump();
    }
}
