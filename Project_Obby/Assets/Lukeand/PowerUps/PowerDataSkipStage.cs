using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Power / SkipStage")]
public class PowerDataSkipStage : PowerData
{

    public override void AddPower()
    {
        //we tell the locarhandler to go to to the next thing.
        PlayerHandler.instance.PlayerWon();
    }

    public override void RemovePower()
    {
        //never removes power.
    }
}
