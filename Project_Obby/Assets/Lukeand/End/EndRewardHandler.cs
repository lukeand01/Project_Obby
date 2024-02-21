using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRewardHandler : MonoBehaviour
{
    [SerializeField] EndRewardUnit rewardGold;
    [SerializeField] EndRewardUnit rewardGoldAd;
    [SerializeField] EndRewardUnit rewardGem;
    [SerializeField] EndRewardUnit rewardGemAd;


    public void CreateRewardGold()
    {
        int coinObtained = LocalHandler.instance.gainedCoin;
        bool hasObtainedAllCoin = coinObtained >= LocalHandler.instance.coins.Length;


        rewardGold.gameObject.SetActive(true);
        rewardGold.SetUp(coinObtained);

        int coinAd = coinObtained;

        if (hasObtainedAllCoin)
        {
            rewardGoldAd.CallFadeUI("You Got all Coins!");
            coinAd = coinObtained + coinObtained;
        }

        rewardGoldAd.gameObject.SetActive(true);
        rewardGoldAd.SetUp(coinAd);
    }


    public void MergeGoldAndAd()
    {
        //do an effect where you merge themtogether.
    }


    public void CreateRewardGem()
    {
        //no gem ad.
        //



    }

    public void CreateRewardGemAd()
    {
        //its always the same value as long as you have completed and not have requested.



    }

}
