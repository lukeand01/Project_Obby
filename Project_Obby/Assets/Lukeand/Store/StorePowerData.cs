using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Store / Power")]
public class StorePowerData : StoreData
{
    public PowerData powerData;

    public override void Buy()
    {
        BaseBuy();
        PlayerHandler.instance.AddPermaPower(powerData);
    }

    //also when we buy that pwoer we pass to a player list so it can deal with it.


    public override StorePowerData GetPower() => this;
    

}
