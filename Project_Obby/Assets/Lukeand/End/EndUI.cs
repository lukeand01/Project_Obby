using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EndUI : MonoBehaviour
{

    GameObject holder;

    //
    [Separator("END")]
    public EndStarUnit StarHolder;
    public EndRewardHandler rewardHolder;
    [SerializeField] EndAchievementUnit achievementHolder;

    [Separator("VICTORY")] 
    [SerializeField] GameObject victoryHolder;
    [SerializeField] Image victoryBackground;
    [SerializeField] GameObject victoryTitleHolder;
    [SerializeField] Transform victoryButtonHolder;


    [Separator("DEFEAT")]
    [SerializeField] GameObject defeatHolder;
    [SerializeField] GameObject defeatUseHealthButton;
    [SerializeField] GameObject defeatRetryStageButton;
    [SerializeField] GameObject defeatMainMenuButton;
    [SerializeField] TextMeshProUGUI stageText;
    [SerializeField] TextMeshProUGUI liveText;
    [SerializeField] ButtonBase useHealthButton;


    Color fullStarColor;
    Color emptyStarColor;

    StageData nextStage = null;
    bool canUseHealth;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;

        fullStarColor = Color.white;
        emptyStarColor = Color.black;

    }

    //if the player has enough health then we show the button.
    //we also check if we should let the player watch an ad.
    //

    #region UI CONTROL
    public void Close()
    {
        holder.SetActive(false);


        StopAllCoroutines();
        StarHolder.StopAllCoroutines();
        rewardHolder.StopAllCoroutines();
        achievementHolder.StopAllCoroutines();
    }

    public void StartVictory()
    {

        holder.SetActive(true);
        victoryHolder.SetActive(true);
        defeatHolder.SetActive(false);


        victoryTitleHolder.transform.position = victoryTitleHolder.transform.position + new Vector3(0, 150, 0);

       var alpha = victoryBackground.color;
        alpha.a = 0;
        victoryBackground.color = alpha;

        StageData currentData = LocalHandler.instance.data;
        GameHandler handler = GameHandler.instance;

        nextStage = handler.stageHandler.GetNextStageData(currentData);

        victoryButtonHolder.gameObject.SetActive(nextStage != null);



        //goldText.text = currentGold + " / " + totalGold;

        StarHolder.gameObject.SetActive(false);
        rewardHolder.gameObject.SetActive(false);

        StartCoroutine(VictoryProcess());

    }



    float StartVictoryTitle()
    {
        float timerForButton = 1.5f;
        Vector3 buttonOffset = new Vector3(0, 150, 0);
        victoryTitleHolder.transform.DOMove(victoryTitleHolder.transform.position - buttonOffset, timerForButton);
        return timerForButton;
    }


    IEnumerator VictoryProcess()
    {

        StageData localData = LocalHandler.instance.data;

        achievementHolder.PutAllPiecesInStartingPos();


       float timeForTitle =  StartVictoryTitle();

        yield return new WaitForSeconds(timeForTitle);

        var alpha = victoryBackground.color;

        while (victoryBackground.color.a < 0.7f)
        {
            alpha.a += 0.01f;
            victoryBackground.color = alpha;
            yield return new WaitForSeconds(0.01f);
        }



        float timerForButton = 0.5f;
        Vector3 buttonOffset = new Vector3(0, 120, 0);
        victoryButtonHolder.DOMove(victoryButtonHolder.transform.position + buttonOffset, timerForButton);

        yield return new WaitForSeconds(timerForButton);

        float timerForHolders = 0.5f;

        StarHolder.MakeAllStarsEmpty();

        StarHolder.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        rewardHolder.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        StarHolder.gameObject.SetActive(true);
        rewardHolder.gameObject.SetActive(true);

        StarHolder.transform.DOScale(0.8f, timerForHolders);
        rewardHolder.transform.DOScale(0.8f, timerForHolders);

     
        yield return new WaitForSeconds(timerForHolders);

        //i want it to come from the thing?


        bool isTitleSuccess = achievementHolder.CallTitle();

        yield return new WaitForSeconds(1);


        if(isTitleSuccess)
        {
            //then we add a heart.
            //if the hearts is new then we also ad the gem.
            yield return StartCoroutine(StarHolder.CallStarProcess(achievementHolder.GetTitlePos()));
        }


        yield return StartCoroutine(achievementHolder.CallCoinProcess());

        int currentCoin = LocalHandler.instance.gainedCoin;
        bool hasAllCoin = currentCoin >= LocalHandler.instance.coins.Length;

        rewardHolder.AddToRewardGold(currentCoin);
        rewardHolder.CreateAdButton(hasAllCoin);

        //create button
        //rewardHolder.CreateAdButton();

        float timeTimer = 1f;

        bool isTimerSuccess = achievementHolder.CallTimer(timeTimer);

        yield return new WaitForSeconds(timeTimer + 0.5f);


        if(isTimerSuccess)
        {
            yield return StartCoroutine(StarHolder.CallStarProcess(achievementHolder.GetTimerPos()));
        }
        else
        {
            //show that timer failed.
        }
        

        float heartTimer = 1;

        bool isHeartSuccess = achievementHolder.CallHeart(heartTimer);

        yield return new WaitForSeconds(heartTimer + 0.5f);

        if (isHeartSuccess)
        {
           yield return StartCoroutine(StarHolder.CallStarProcess(achievementHolder.GetHeartPos()));
        }
        else
        {
            //show that it failed.
        }

        yield return new WaitForSeconds(0.5f);
        


    }

    //we only really have to ask in the last if the player got all stars.
    


    //so now we will show the achievements as we give stars
    //then each star grants five.


    public void StartDefeat(int currentHealth, bool hasAlreadyWatchedAD)
    {       

        //what do i do here?

        //i have potenl



        holder.SetActive(true);
        victoryHolder.SetActive(false);
        defeatHolder.SetActive(true);

        liveText.text = "Lives: " + currentHealth.ToString();


        useHealthButton.gameObject.SetActive(currentHealth > 0 || !hasAlreadyWatchedAD);

        canUseHealth = currentHealth > 0;

        if(currentHealth > 0)
        {
            useHealthButton.SetText("Use Health");
        }
        else if (!hasAlreadyWatchedAD)
        {
            useHealthButton.SetText("Watch an ad");
        }



    }

    #endregion

    #region DECIDE

    //really important. when we decide on things we should pass the value.


    public void DecidedToNextStage()
    {
        //ccheck if you can go to the next.
        if(nextStage == null)
        {
            Debug.Log("next stage is null");
            return;
        }
        LocalHandler.instance.CompleteStage();
        //here we will also tell localhandler to give all the values to the player.
        GameHandler.instance.sceneLoader.ChangeScene(nextStage);
    }
    public void DecideUseHealth()
    {
        if (canUseHealth)
        {
            //cost health and reload.
            PlayerHandler.instance.RespawnUsingHealth();
        }
        else
        {
            //watch an ad and does the same.
            //just call the ad for now i wont care.
        }

    }
    public void DecideStartStageFromStart(bool HasWon)
    {
        //

        if (HasWon)
        {
            LocalHandler.instance.CompleteStage();
        }


        PlayerHandler.instance.RespawnUsingNothing();
    }
    public void DecideGoBackToMenu(bool HasWon)
    {
        if (HasWon)
        {
            LocalHandler.instance.CompleteStage();
        }


        GameHandler.instance.sceneLoader.ChangeToMainMenu();
    }
    
    public void DecideWatchGoldAd()
    {
        GameHandler.instance.adHandler.RequestRewardAd(RewardType.ModifyCoinValue);
    }

    #endregion


    //

    
}
///new victory progress
///first we show the title.
///we remove all the other ui
///then we let him dance 
///then call the transparent beackground
///then we list all the achievements.
///as we do the achievemnts we call things
///when it shows all teh coin grabbed. then we create a reward for it
///we also create a reward ad which shows the additional coins to be gained by doing which can be 2x or 3x
///it gives a star at first.
///it gives a star after timer and after health if conditions are met.
//everytime is a new star there will be a fade ui saying 'new star' and the player will gain gems
//five per star. it appears above and appear by the rewards.
//when we are no longer counting the gems if it is 
//we need to count the coin.
//


//and then we count the coin
//and then we count the stars.
//then each new star gained gives gems