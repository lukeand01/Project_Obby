using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Store / Animation")]
public class StoreAnimationData : StoreData
{

    public AnimationType animationType;


    public override void Buy()
    {
        //we give this index to the player
        Debug.Log("this was called");
        BaseBuy();
        PlayerHandler.instance.graphic.SetAnimationIndex((int)animationType);
    }

    public override StoreAnimationData GetAnimation() => this;
    
}
