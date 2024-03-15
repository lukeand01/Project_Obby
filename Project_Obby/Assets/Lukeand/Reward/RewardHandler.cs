using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardHandler : MonoBehaviour
{

    [SerializeField] List<RewardData> rewardList = new();
    DateTime lastDailyRewardTime;
    int currentDailyRewardIndex;




    private void Update()
    {
        //we keep checking if 

        //DebugErrorText.Log(GetRemainingTimeForDailyReward().Hours);

        //DebugErrorText.Log(GetRemainingTimeForDailyReward().ToString());

        // DebugErrorText.Log("last ime " + lastDailyRewardTime.ToString());


        return;

        if (currentDailyRewardIndex <= 0) return;

        
        if (GetRemainingTimeForDailyReward().Day >= 2)
        {
            //its gone.
            ResetIndex();
        }
        

        

    }


    public void SetRewardHandler(SaveClass saveData)
    {
 
        //DebugErrorText.Log("the save data is " + saveData.da)

        lastDailyRewardTime = saveData.dailyRewardLastTime;

        if(GetRemainingTimeForDailyReward().Day >= 2)
        {
            ResetIndex();
        }
    }


    public void GetNewData()
    {
        //here we give a fresh information.
        //and so we should be able to instnatnly pick the thing

        lastDailyRewardTime = DateTime.UtcNow.AddDays(-1);

        

    }



    public DateTime GetRemainingTimeForDailyReward()
    {
        DateTime timer = DateTime.UtcNow;

        int dayDiff = timer.Day - lastDailyRewardTime.Day;
        int hourDiff = timer.Hour - lastDailyRewardTime.Hour ;
        int minuteDiff = timer.Minute - lastDailyRewardTime.Minute ; 
        int secondDiff = timer.Second - lastDailyRewardTime.Second ;


        //DateTime newTimer = new DateTime(1, 1, dayDiff, hourDiff, minuteDiff, secondDiff);


        //TimeSpan newTimer = new TimeSpan(lastDailyRewardTime.Hour - timer.Hour , lastDailyRewardTime.Minute - timer.Minute , lastDailyRewardTime.Second - timer.Second);

        //DebugErrorText.Log("new timespawn " + newTimer.ToString());

        return DateTime.UtcNow;
    }


    TimeSpan GetTimerToShow()
    {

        return new TimeSpan();

    }

    public bool CanPickDailyReward()
    {
        DateTime differenceTimer = GetRemainingTimeForDailyReward();

        //DebugErrorText.Log("this is the difference timer " + differenceTimer.Hours);

        return differenceTimer.Day >= 1;


    }


    public void GrantDailyReward()
    {
        RewardData data = rewardList[currentDailyRewardIndex];
        data.Buy();
           
        
        if(MainMenuUI.instance == null)
        {
            Debug.LogError("main menu instance was called outside the mainscene or the instance failed");
            return;
        }

        MainMenuUI.instance.rewardUI.StartRewardUI(data);

        AddToIndex();
    }

    void AddToIndex()
    {
        currentDailyRewardIndex += 1;

        if(currentDailyRewardIndex >= rewardList.Count)
        {
            currentDailyRewardIndex = 0;
        }
    }

    void ResetIndex()
    {
        currentDailyRewardIndex = 0;
    }



}
