using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Power / Shield")]
public class PowerDataShield : PowerData
{

    public override void AddPower()
    {
        base.AddPower();
        PlayerHandler.instance.ReceiveShield();
    }

    public override void RemovePower()
    {
        PlayerHandler.instance.RemoveShield();
    }
}
