using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndGoldUnit : ButtonBase
{
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI bonusModifierText;
    [SerializeField] GameObject bonusModifierHolder;

    bool alreadyClicked;

    int coinsTotalGlobal = 0;
    int coinsCurrentGlobal = 0;

    int coinModifier = 0;

    public IEnumerator CountCoinProcess()
    {

        Debug.Log("this was called");

        yield break;


        alreadyClicked = false;

        int coinsReserved = LocalHandler.instance.gainedCoin;
        coinsCurrentGlobal = coinsReserved;

        int coinsCurrent = 0;

        int coinsTotal = LocalHandler.instance.coins.Length;
        coinsTotalGlobal = coinsTotal;
        


        goldText.text = $"{coinsCurrent} / {coinsTotal}";


        if(coinsCurrent >= coinsTotal)
        {
            //then we say that we can get 3x modifier from watching ads.
            coinModifier = 3;
        }
        else
        {
            //otherwise its just 2x
            coinModifier = 2;
        }

        bonusModifierHolder.SetActive(true);
        bonusModifierText.text = coinModifier.ToString() + "X";

        while(coinsReserved > 0)
        {
            coinsReserved--;
            coinsCurrent++;
            goldText.text = $"{coinsCurrent} / {coinsTotal}";
            yield return new WaitForSeconds(0.05f);
        }





        yield return null;
    }

    public IEnumerator CoinMultiplierProcess(int additionalValue)
    {

        int coinReserve = additionalValue;


        while(coinReserve > 0)
        {
            coinReserve--;
            coinsCurrentGlobal++;
            goldText.text = $"{coinsCurrentGlobal} / {coinsTotalGlobal}";

            yield return new WaitForSeconds(0.05f);
        }

        //the value itself has already been accepted.
        //now very importantly is that we need to give these values.

    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (alreadyClicked) return;

        Debug.Log("yo");
        alreadyClicked = true;

        base.OnPointerClick(eventData);
        //if you click on it it will call for a
        StopAllCoroutines();
        goldText.text = $"{coinsCurrentGlobal} / {coinsTotalGlobal}";
        //we call an ad
        GameHandler.instance.adHandler.RequestRewardAd(RewardType.ModifyCoinValue, coinModifier);
    }

    //if i click on this should i stop the rest of skip it?


}
