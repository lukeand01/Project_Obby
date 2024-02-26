using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Store / Animation")]
public class StoreAnimationData : StoreData
{

    public AnimationType animationType;


    public override void Buy()
    {
        
    }

    public override StoreAnimationData GetAnimation() => this;
    
}
