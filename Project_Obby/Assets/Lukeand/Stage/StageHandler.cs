using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageHandler : MonoBehaviour
{
    //this is going to deal with how much time you are spending in this


    public List<StageData> stageList = new(); //these are all stages in the game.    

    StageTimeClass totalTime;

    int currentSeconds;
    int currentMinutes;

    public Action eventTimeRanOut;

    public void CallEventTimeRanOut()
    {
        eventTimeRanOut?.Invoke();
    }


    public StageData GetNextStageData(StageData currentStage)
    {
        if(currentStage == null)
        {
            Debug.Log("yo");
            return null;
        }

        int index = currentStage.stageId - 2;

        Debug.Log("this is the etarget index " + index);

        if(index + 1 >= stageList.Count)
        {
            Debug.Log("there was nothing here");
            return null;
        }
        else
        {
            return stageList[index + 1];
        }


    }


    //we save the time in every spawn.
    //


    #region TIMER
    public void SetTImer(StageTimeClass totalTime)
    {
        this.totalTime = totalTime;

        currentSeconds = totalTime.seconds;
        currentMinutes = totalTime.minutes;

    }

    public void StartTimer()
    {
        StartCoroutine(TimerProcess());
    }
    public void ResetTimer()
    {
        currentMinutes = totalTime.minutes;
        currentSeconds = totalTime.seconds;
    }

    public StageTimeClass GetTimer()
    {
        StageTimeClass time = new StageTimeClass(currentMinutes, currentSeconds);

        return time;
    }

    IEnumerator TimerProcess()
    {
        while (currentMinutes > 0 || currentMinutes > 0)
        {
            //while either are true we count

            currentSeconds -= 1;

            if(currentSeconds <= 0)
            {
                currentSeconds = 60;
                currentMinutes -= 1;
            }

            UIHandler.instance.uiPlayer.UpdateTimerUI(currentMinutes, currentSeconds);

            yield return new WaitForSeconds(1);
        }

        currentSeconds = 1;
        currentMinutes = 0;
        UIHandler.instance.uiPlayer.UpdateTimerUI(currentMinutes, currentSeconds);
        yield return new WaitForSeconds(2);

        //if it gets here the timer is over and we 
        CallEventTimeRanOut();
        currentSeconds = 0;
        UIHandler.instance.uiPlayer.UpdateTimerUI(currentMinutes, currentSeconds);

    }

    #endregion


}
