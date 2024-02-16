using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RewardData : ScriptableObject
{
    //what can be gained?

    public string rewardName;
    public Sprite rewardSprite;
    public int rewardQuantity;

    [SerializeField] RewardType rewardType;
     enum RewardType
    {
        Gold,
        Stars,
    }

    public void Buy()
    {
        if(rewardType == RewardType.Gold)
        {
            //we play an effect about what you gained.
            //then add the gold.

        }

        if(rewardType == RewardType.Stars)
        {

        }


    }


}

