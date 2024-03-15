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

    public List<SaveClassStage> GetSaveClassStageList()
    {

        //actually we only save up to playerprogress. that way we dont save unecessary stuff.
        List<SaveClassStage> stageSaveList = new();
        int playerProgress = PlayerHandler.instance.stageProgress;



        for (int i = 0; i < playerProgress; i++)
        {
            if(i > stageList.Count)
            {
                Debug.Log("problem here");
                continue;
            }

            StageData stageData = stageList[i];
            int bestMinute = 0;
            int bestSecond = 0;

           if(stageData.stageCompletedTimer != null)
            {
                bestMinute = stageData.stageCompletedTimer.minutes;
                bestSecond = stageData.stageCompletedTimer.seconds;
            }


            SaveClassStage save = new SaveClassStage(i, stageData.stageStarGained, bestMinute, bestSecond);
            stageSaveList.Add(save);
        }


        return stageSaveList;
    }

    public void ReceiveStageDataList(List<SaveClassStage> saveClassStageList)
    {
        if (saveClassStageList == null) return;


        //here is the problem,


        for (int i = 0; i < saveClassStageList.Count; i++)
        {
            if (i > stageList.Count)
            {
                Debug.Log("there are more itens in savelist than stagelist");
                return;
            }

            StageData data = stageList[i];
            SaveClassStage save = saveClassStageList[i];


           

            if(data == null)
            {
                Debug.Log("there was no data here");
                continue;
            }

            if(save.stageIndex != i)
            {
                Debug.Log("there was something wrong");
                continue;
            }


            data.ReceiveSaveData(save.stageStarsQuantity, save.bestTimerMinute, save.bestTimerSecond);


        }


        for (int i = saveClassStageList.Count; i < stageList.Count; i++)
        {
            stageList[i].ResetStage();
        }
    }

    public void ResetAllStages()
    {
        foreach (var item in stageList)
        {
            item.ResetStage();
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
