using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Store / StorePower")]
public class StoreDataPermaPower : StoreData
{
    [SerializeField] PowerType power;
    public override void Buy()
    {
        throw new System.NotImplementedException();
    }

    //here we check if we already have this power. money is always checked before this.
    public override bool CanBuy()
    {
        throw new System.NotImplementedException();
    }
}

public enum PowerType
{
    Shield = 0,
    Jetpack = 1,
    Spring = 2
}