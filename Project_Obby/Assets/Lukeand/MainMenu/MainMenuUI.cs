using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    //will handle stage stuff here.

    //[SerializeField] GameObject DEBUGStageHolder;

    public static MainMenuUI instance;


    [SerializeField] List<GameObject> holderLIst = new();

    [Separator("REFERENCES TO UI")]
    public PlayUI playUI;
    public StoreUI storeUI;
    public InventoryUI inventoryUI;
    public RewardUI rewardUI;
    public ReportUI reportUI;


    [Separator("PLAYER STUFF")]
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI starText;

    [Separator("WARNING")]
    [SerializeField] TextMeshProUGUI warnText;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //look for the stage handler.

        if(GameHandler.instance == null)
        {
            Debug.Log("there was no gamehandler for some reason");
            return;
        }

        //the banner is taking the ui in the top.
        GameHandler.instance.adHandler.RequestBanner();

        playUI.CreateStageUnits(GameHandler.instance.stageHandler.stageList);
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
        starText.text = "Lives: " + PlayerHandler.instance.star.ToString();

    }


  

    #region CHANGE WORLDS

    void ChangeWorld()
    {

    }
    void CalculateTotalStarForWorld()
    {

    }


    #endregion

    #region STORE



    #endregion

    #region PURCHASE

    public void PurchaseCompletedForGold(int value)
    {

    }

    #endregion

    public void SetWarn(string text)
    {
        warnText.text = text;
    }
}

public enum MainMenuHolderType
{
    MainMenu = 0,
    Play = 1,
}