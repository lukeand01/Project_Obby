using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EndUI : MonoBehaviour
{

    GameObject holder;

    //
    [Separator("END")]
    [SerializeField] EndStarUnit StarHolder;
    [SerializeField] public EndGoldUnit goldHolder;


    [Separator("VICTORY")] 
    [SerializeField] GameObject victoryHolder;
    [SerializeField] GameObject victoryNextStageButton;
    [SerializeField] GameObject victoryRetryStageButton;
    [SerializeField] GameObject victoryMainMenuButton;


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
    }

    public void StartVictory()
    {

        holder.SetActive(true);
        victoryHolder.SetActive(true);
        defeatHolder.SetActive(false);

        StageData currentData = LocalHandler.instance.data;
        GameHandler handler = GameHandler.instance;

        nextStage = handler.stageHandler.GetNextStageData(currentData);

        victoryNextStageButton.SetActive(nextStage != null);


        int currentGold = 0;
        int totalGold = 0;

        int starsGained = 0;


        //goldText.text = currentGold + " / " + totalGold;
        StarHolder.gameObject.SetActive(false);
        goldHolder.gameObject.SetActive(false);




        StartCoroutine(VictoryProcess());

    }

    IEnumerator VictoryProcess()
    {
        //first we call everybutton up from its hiding spot.
        //scale up both gold and stars.




        float timerForButton = 1.5f;
        Vector3 buttonOffset = new Vector3(0, 150, 0);
        victoryNextStageButton.transform.DOMove(victoryNextStageButton.transform.position + buttonOffset, timerForButton);
        victoryRetryStageButton.transform.DOMove(victoryRetryStageButton.transform.position + buttonOffset, timerForButton);
        victoryMainMenuButton.transform.DOMove(victoryMainMenuButton.transform.position + buttonOffset, timerForButton);

        yield return new WaitForSeconds(timerForButton);


        float timerForHolders = 0.5f;

        StarHolder.MakeAllStarsEmpty();

        StarHolder.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        goldHolder.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        StarHolder.gameObject.SetActive(true);
        goldHolder.gameObject.SetActive(true);

        StarHolder.transform.DOScale(1, timerForHolders);
        goldHolder.transform.DOScale(1, timerForHolders);


        yield return new WaitForSeconds(timerForHolders);


        //coin the coins.
        yield return StartCoroutine(goldHolder.CountCoinProcess());


        yield return StartCoroutine(StarHolder.GetStarFromPlacesProcess());


    }


    public void StartDefeat(int currentHealth, bool hasAlreadyWatchedAD)
    {       
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
    public void DecidedToNextStage()
    {
        //ccheck if you can go to the next.
        if(nextStage == null)
        {
            Debug.Log("next stage is null");
            return;
        }
       
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
    public void DecideStartStageFromStart()
    {
        //
        PlayerHandler.instance.RespawnUsingNothing();
    }
    public void DecideGoBackToMenu()
    {
        GameHandler.instance.sceneLoader.ChangeToMainMenu();
    }
    public void DecideThisStageAsCompleted()
    {
        //if the player wants to retry the stage it will be still be seen as completed.
        PlayerHandler.instance.ChangeProgress(1);
    }

    #endregion


    //

    public Vector3 GetCoinPos() => goldHolder.transform.position;
    
}
