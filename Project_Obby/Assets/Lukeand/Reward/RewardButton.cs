using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RewardButton : ButtonBase
{
    //


    RewardHandler handler;
    bool canTakeReward;

    [SerializeField] TextMeshProUGUI rewardTimeLeftText;

    private void Start()
    {
        handler =  GameHandler.instance.rewardHandler;
    }

    private void Update()
    {
        //keep checking the amount of time left.

        canTakeReward = handler.CanPickDailyReward();

        rewardTimeLeftText.gameObject.SetActive(!canTakeReward);

        if (canTakeReward)
        {
            //then show a different animation.
        }
        else
        {
            //then we show the amount of time left.
            DateTime timeLeft = handler.GetRemainingTimeForDailyReward();
            UpdateText(timeLeft);
        }

    }

    void UpdateText(DateTime timeLeft)
    {
        int hours = timeLeft.Hour;
        int minutes = timeLeft.Minute;
        int seconds = timeLeft.Second;

        if(hours == 0 && minutes == 0)
        {
            rewardTimeLeftText.text = "0 : " + seconds;
        }
        else
        {
            rewardTimeLeftText.text = hours + " : " + minutes;
        }


    }


    public override void OnPointerClick(PointerEventData eventData)
    {


        base.OnPointerClick(eventData);

       if(canTakeReward)
        {
            //we call ad.
            GameHandler.instance.adHandler.RequestRewardAd(RewardType.DailyReward);
        }
        else
        {

        }



    }


}
