using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StageData : ScriptableObject
{
    public string stageName;
    public int stageId;
    public bool hasAlreadyRequestedGemAd;


    public StageTimeClass stageLimitTimer;
    public StageTimeClass stageCompletedTimer {  get; private set; }

    public int stageStarGained {  get; private set; }

    public void SetStarGained(int starGained)
    {
        if(starGained > stageStarGained)
        {
            stageStarGained = starGained;
        }
    }


    //what are the stars based on?
    //based in not losing health and based in completing 
    //0 stars if you lost 2 health and another starts if you completed before half the timer.


    
    


}

[System.Serializable]
public class StageTimeClass
{
    public int minutes;
    public int seconds;

    int originalMinutes;
    int originalSeconds;


    public StageTimeClass(int minutes, int seconds)
    {
        this.minutes = minutes;
        originalMinutes = minutes;
        this.seconds = seconds;
        originalSeconds = seconds;
    }

    public void CountTimeDown()
    {
        seconds -= 1;

        if(seconds < 0 && minutes >= 1)
        {
            seconds = 60;
            minutes -= 1;
        }
    }

    public bool HasSomething()
    {
        return minutes != 0 && seconds != 0;
    }

    public bool TimeLeft()
    {
        int totalValue = minutes + seconds;
        return totalValue != 0;
    }

    public bool LittleTimeLeft()
    {
        int totalValue = minutes + seconds;
        return totalValue <= 10;
    }

    public bool IsCurrentMoreThanHalfTheOriginal()
    {
        int totalValue = (minutes * 60) + seconds;
        int originalTotalValue = (originalMinutes * 60) + originalSeconds;


        return totalValue > originalTotalValue / 2;

    }


}