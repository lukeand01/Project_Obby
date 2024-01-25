using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    //will handle stage stuff here.

    [SerializeField] GameObject DEBUGStageHolder;
    [SerializeField] Transform DEBUGStageContainer;
    [SerializeField] StageUnit DEBUGStageTemplate;
    [SerializeField] List<GameObject> holderLIst = new();

    [Separator("PLAYER STUFF")]
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI livesText;

    [Separator("DESCRIPTION")]
    [SerializeField] GameObject descriptionHolder;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI timeLimitText;
    [SerializeField] TextMeshProUGUI timeAlreadyDoneText;

    [Separator("STORE")]
    [SerializeField] GameObject storeHolder;


    private void Start()
    {
        //look for the stage handler.

        if(GameHandler.instance == null)
        {
            Debug.Log("there was no gamehandler for some reason");
            return;
        }

        
        CreateStageUnits(GameHandler.instance.stageHandler.stageList);
    }


    public void OpenWithIndex(int index)
    {
        CloseAll();
        holderLIst[index].SetActive(true);
    }

    void CloseAll()
    {
        foreach (var item in holderLIst)
        {
            item.SetActive(false);
        }
    }

    public void UpdatePlayerStats()
    {
        //update both lives and gold.

        goldText.text = "Gold: " + PlayerHandler.instance.gold.ToString();
        livesText.text = "Lives: " + PlayerHandler.instance.currentHealth;

    }


    #region HANDLE STAGES




    void CreateStageUnits(List<StageData> stageList)
    {
        foreach (StageData stageData in stageList)
        {
            StageUnit newObject = Instantiate(DEBUGStageTemplate, Vector2.zero, Quaternion.identity);
            newObject.SetUpStage(this, stageData);
            newObject.transform.parent = DEBUGStageContainer.transform;


        }

    }

    StageUnit currentStageUnit;
    public void SelectStageUnit(StageUnit stageUnit)
    {

        if(currentStageUnit != null)
        {
            currentStageUnit.Unselect();
        }

        currentStageUnit = stageUnit;
        currentStageUnit.Select();
        DescribeStage();
    }


    void DescribeStage()
    {
        if(currentStageUnit == null)
        {
            Debug.Log("problem");
        }
        if (currentStageUnit.data == null)
        {
            Debug.Log("problem 2");
        }

        StageData data = currentStageUnit.data;




        nameText.text = data.stageName;

        string firstMinute = data.stageLimitTimer.minutes.ToString();
        string firstSecond = data.stageLimitTimer.seconds.ToString();

        timeLimitText.text = "Time Limit: " + firstMinute + ":" + firstSecond;


        return;
        if (!data.stageCompletedTimer.HasSomething())
        {
            timeAlreadyDoneText.gameObject.SetActive(false);
            return;
        }

        timeAlreadyDoneText.gameObject.SetActive(true);

        string secondMinute = data.stageCompletedTimer.minutes.ToString();
        string secondSecond = data.stageCompletedTimer.seconds.ToString();

        timeAlreadyDoneText.text = secondMinute + ":" + secondSecond;
    }

    public void PlayCurrentStage()
    {
        if(currentStageUnit == null)
        {
            Debug.Log("this was null");
            return;
        }

        //load this stage.
        GameHandler.instance.sceneLoader.ChangeScene(currentStageUnit.data);
    }


    #endregion


    #region STORE



    #endregion

    #region PURCHASE

    public void PurchaseCompletedForGold(int value)
    {

    }

    #endregion


}
