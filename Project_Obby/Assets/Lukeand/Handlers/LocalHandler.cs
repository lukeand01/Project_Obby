using MyBox;
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

    //these two are the values that will be given. the only real values.
    public int gainedCoin {  get; private set; }
    public int gainedGems { get; private set; }

    int gainedStars;

    [Separator("DEBUG")]
    [SerializeField] bool debugDoNotCallPresentation;


    public StageTimeClass currentTimer {  get; private set; }

    TouchPower[] allTouchPowers;


    private void Awake()
    {
        instance = this;

        //this is heavy so it can be done only once and at the beginning.
        allTouchPowers = FindObjectsOfType<TouchPower>();

        //we remove the fellas.

        foreach (var item in allTouchPowers)
        {
            if (item.data.GetPower() == null)
            {
                Debug.Log("no power here");
                return;
            }

            if (PlayerHandler.instance.HasPermaPower(item.data.GetPower().powerData))
            {
                Destroy(item.gameObject);
            }
            

            
        }


        //at the start we get a list of all power stuff and we change them.
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

    public void CompleteStage()
    {
        //coin
        //gem

        PlayerHandler.instance.ChangeCoin(gainedCoin);
        PlayerHandler.instance.ChangeGem(gainedGems);
        PlayerHandler.instance.ChangeProgress();

        //and we give the stage stars to the data.
        //and when we load another scene we save all the data.

        Debug.Log("this was the quantity of gained stars " + gainedStars);

        data.SetStarGained(gainedStars);
        data.SetNewRecord(currentTimer);
    }

    public void StartLocalHandler(StageData stage, StageTimeClass forcedTimer = null)
    {
        data = stage;

        if(forcedTimer != null)
        {
            Debug.Log("tried to call with a save");
            currentTimer = forcedTimer;
        }
        else
        {
            currentTimer = new StageTimeClass(stage.stageLimitTimer.minutes, stage.stageLimitTimer.seconds);
        }

        
      
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

        if (debugDoNotCallPresentation)
        {
            PlayerHandler.instance.cam.ResetCam();
        }
        else
        {
            StartCoroutine(StartStageProcess());
        }

        
        
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
        //StartCoroutine(handler.cam.CamIntroductionProcess());
        handler.cam.CallCamIntroductionProcess();

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

            float timerModifier = PlayerHandler.instance.TimerModifier;

            yield return new WaitForSeconds(1 * timerModifier);
        }
        UIHandler.instance.uiPlayer.LeaveTimerRed();
        PlayerHandler.instance.OrderToEndGameFromTimer();
        //we must also end the game
        


    }

    

    public void MultiplyCoinGained()
    {

        //we will create the whole logic here.
        bool gotAllCoins = gainedCoin >= coins.Length - 1;
        int additionalQuantity = 0;

        if(gotAllCoins)
        {
            additionalQuantity = gainedCoin * 2;
        }
        else
        {
            additionalQuantity = gainedCoin;
        }

        gainedCoin += additionalQuantity;

        UIHandler.instance.uiEnd.rewardHolder.AddToRewardGold(additionalQuantity);
    }


    public void AddGem(int value)
    {
        gainedGems += value;
    }


    public void MultiplyGemGained()
    {
        //we get the stuff here. only here.
        gainedGems += 15;

        //its always just 15.

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
        //i want to pass information regarding the time the player died.
        GameHandler.instance.sceneLoader.ResetScene(data, currentTimer);
    }


    //the problem is that i probably want to especify why i gained each star.
    public void CalculateGainedGems()
    {
        int starsAlreadyObtained = data.stageStarGained;
        int counting = 0;

        counting += 1;

        if(PlayerHandler.instance.currentHealth == 3)
        {
            counting += 1;
        }

        if(currentTimer.IsCurrentMoreThanHalfTheOriginal())
        {
            counting += 1;
        }

        gainedStars = counting;
        int newlyAcquiredStars = -starsAlreadyObtained;
        newlyAcquiredStars += counting;

        Debug.Log("got new stars " + newlyAcquiredStars);

        for (int i = 0; i < newlyAcquiredStars; i++)
        {
            AddGem(5);
        }

    }


    public void StopEverything()
    {
        StopAllCoroutines();
    }
}
