using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Store / Health")]
public class StoreDataHealth : StoreData
{
    //either max health or current health. currentHealth can only be bought once.


    public override void Buy()
    {
        throw new System.NotImplementedException();
    }

    public override bool CanBuy()
    {
        throw new System.NotImplementedException();
    }
}
