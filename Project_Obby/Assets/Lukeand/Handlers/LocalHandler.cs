using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class LocalHandler : MonoBehaviour
{
    public static LocalHandler instance;


    //

    public StageData data {  get; private set; }
    [SerializeField] List<SpawnPoint> spawnPointList = new();
    [SerializeField] StageData debugStart;

    [SerializeField] List<TouchCoin> localCoinList = new();


    

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (debugStart)
        {
            StartLocalHandler(debugStart);
            //i will call to fix the playerposition here just for testing.
            PlayerHandler.instance.DebugSpawnInLastPosition();


            //i need to start the counter. i also need 

        }

        

    }


    //localhandler will just hold the list and the playter will use it.

    StageTimeClass currentTimer;

    public void StartLocalHandler(StageData stage)
    {
        data = stage;

        currentTimer = new StageTimeClass(stage.stageLimitTimer.minutes, stage.stageLimitTimer.seconds);

        

        for (int i = 0; i < spawnPointList.Count; i++)
        {
            spawnPointList[i].SetIndex(i);
        }

        for (int i = 0; i < localCoinList.Count; i++)
        {
            localCoinList[i].SetIndex(i);
        }

        //then we get information here regarding to the saved data.

        StopAllCoroutines();
        StartCoroutine(StartStageProcess());
    }

    IEnumerator StartStageProcess()
    {

        UIHandler.instance.uiPlayer.ShowTimer();
        yield return new WaitForSeconds(1.5f);



        while (currentTimer.TimeLeft())
        {
            currentTimer.CountTimeDown();
            UIHandler.instance.uiPlayer.UpdateTimerUI(currentTimer.minutes, currentTimer.seconds);

            if (currentTimer.LittleTimeLeft())
            {
                UIHandler.instance.uiPlayer.TriggerTimerRedWarning();
            }

            yield return new WaitForSeconds(1);
        }
        UIHandler.instance.uiPlayer.LeaveTimerRed();

        yield return new WaitForSeconds(0.1f);

    }


    public void CompleteStage()
    {
        StopAllCoroutines();

        int starsGained = 0;

        int currentHealth = PlayerHandler.instance.currentHealth;
        
        if(localCoinList.Count == 0)
        {
            starsGained++;
        }

        if(currentHealth >= 3)
        {
            starsGained++;
        }

        if (currentTimer.IsCurrentMoreThanHalfTheOriginal())
        {
            starsGained++;
        }

        data.SetHeartGained(starsGained);

    }
   
    public SpawnPoint GetRightSpawnPoint(int index)
    {
        if(index > spawnPointList.Count)
        {
            Debug.Log("there was a problem here");
            return null;
        }

        RemoveAllSpawnsPriorToTheCurrentOne(index);
        spawnPointList[index].MakeItUsed();
        return spawnPointList[index];
    }


    void RemoveAllSpawnsPriorToTheCurrentOne(int index)
    {
        for (int i = 0; i < index; i++)
        {
            spawnPointList[i].MakeItUsed();
        }
    }


    public void ResetScene()
    {
        GameHandler.instance.sceneLoader.ResetScene(data);
    }
}
