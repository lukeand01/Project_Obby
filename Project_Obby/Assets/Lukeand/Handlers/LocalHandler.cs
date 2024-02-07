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

    public TouchCoin[] coins { get; private set; }
    public int gainedCoin {  get; private set; }
        //we put here because we only give to the player when he wins it.

    

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


        coins = GameObject.FindObjectsOfType<TouchCoin>();
        gainedCoin = 0;



        for (int i = 0; i < coins.Length; i++)
        {
            coins[i].SetIndex(i);
        }

        //then we get information here regarding to the saved data.




        StopAllCoroutines();
        StartCoroutine(StartStageProcess());
    }

    public void AddLocalCoin(int value)
    {
        gainedCoin += value;
    }

    public void StopTimer()
    {
        StopAllCoroutines();
    }

    IEnumerator StartStageProcess()
    {

        //then we lock it again till we are done.
        //we have the camera do the thing.
        //and we have the timer do it as well.
        //i need to be at the same time they all come together.

        //i will fix 


        PlayerHandler handler = PlayerHandler.instance;
        PlayerUI playerUI = UIHandler.instance.uiPlayer;

        handler.controller.blockClass.AddBlock("StartStage", BlockClass.BlockType.Complete);
        handler.cam.ResetCamToIntroduction();
        handler.ForceRightRotationInRelationToSpawn();
        StartCoroutine(handler.cam.CamIntroductionProcess());

        playerUI.ShowTimer();


        //the camera should be behind the player

        for (int i = 3; i > 0; i--)
        {
            playerUI.UpdateTimerStringUI(i.ToString());
            yield return new WaitForSeconds(0.8f);
        }

        playerUI.UpdateTimerStringUI("GO");
        StartCoroutine(playerUI.TimerAnimationProcess());
        yield return new WaitForSeconds(1f);

       
    

        StartCoroutine(CountTimerProcess());

        //handler.cam.ResetCam();


        handler.controller.blockClass.RemoveBlock("StartStage");

    }

    IEnumerator CountTimerProcess()
    {
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
    }

    public void CompleteStage()
    {
        StopAllCoroutines();

        int starsGained = 0;

        int currentHealth = PlayerHandler.instance.currentHealth;

        if (GainedStarByCoin())
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

    public void MultiplyGoinGained(int value)
    {

        int additionalValue = (gainedCoin * value) - gainedCoin;

        gainedCoin *= value;


       StartCoroutine(UIHandler.instance.uiEnd.goldHolder.CoinMultiplierProcess(additionalValue));
    }


    public bool GainedStarByCoin()
    {
    
        return gainedCoin >= coins.Length;
    }
    public bool GainedStarByHealth()
    {
        int currentHealth = PlayerHandler.instance.currentHealth;
        return currentHealth >= 3;
    }
    public bool GainedStarByTimer()
    {
        return currentTimer.IsCurrentMoreThanHalfTheOriginal();
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
