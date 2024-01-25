using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class EndUI : MonoBehaviour
{

    GameObject holder;

    [Separator("VICTORY")] 
    [SerializeField] GameObject victoryHolder;
    [SerializeField] GameObject nextStageButton;

    [Separator("END")]
    [SerializeField] GameObject defeatHolder;
    [SerializeField] TextMeshProUGUI stageText;
    [SerializeField] TextMeshProUGUI liveText;
    [SerializeField] ButtonBase useHealthButton;


    StageData nextStage = null;
    bool canUseHealth;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
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

        nextStageButton.SetActive(nextStage != null);
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


}
