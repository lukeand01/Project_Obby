using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Store / Power")]
public class StorePowerData : StoreData
{
    [SerializeField] PowerData powerData;

    public override void Buy()
    {
        PlayerHandler.instance.AddPower(powerData);
    }

    
}
