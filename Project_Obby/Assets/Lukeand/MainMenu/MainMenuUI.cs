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

    [SerializeField] Camera cameraCanvas;

    [SerializeField] List<GameObject> holderLIst = new();

    [Separator("REFERENCES TO UI")]
    public PlayUI playUI;
    public StoreUI storeUI;
    public InventoryUI inventoryUI;
    public RewardUI rewardUI;
    public ReportUI reportUI;


    [Separator("PLAYER STUFF")]
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI gemText;

    [Separator("WARNING")]
    [SerializeField] TextMeshProUGUI warnText;

    [Separator("AUDIO CLIP")]
    [SerializeField] AudioClip bgMusic;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //look for the stage handler.



        if (GameHandler.instance == null)
        {
            return;
        }


        //the banner is taking the ui in the top.




        //GameHandler.instance.adHandler.RequestBanner();



        UpdatePlayerCurrencies();    

        playUI.CreateStageUnits2(GameHandler.instance.stageHandler.stageList);


        GameHandler.instance.soundHandler.ChangeBackgroundMusic(bgMusic, true);


        Application.logMessageReceived += DebugErrorText;


    }





    public void UpdatePlayerCurrencies()
    {



        goldText.text = PlayerHandler.instance.coins.ToString();
        gemText.text = PlayerHandler.instance.gems.ToString();
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
        //update both lives and coins.

        goldText.text = "Gold: " + PlayerHandler.instance.coins.ToString();
        gemText.text = "Lives: " + PlayerHandler.instance.gems.ToString();

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


    #region DEBUG
    [Separator("DEBUG")]
    [SerializeField] TextMeshProUGUI debugText;
    [SerializeField] TextMeshProUGUI debugErrorText;
    [SerializeField] Transform debugContainer;

    public void DebugErrorText(string logString, string stackTrace, LogType type)
    {

        if(type == LogType.Error)
        {
            Debug.Log("this is error");
            debugErrorText.text = logString;
        }

        
    }

    public void DebugConsoleText(string value)
    {

        TextMeshProUGUI newObject = Instantiate(debugText, Vector2.zero, Quaternion.identity);
        newObject.text = value;
        newObject.transform.SetParent(debugContainer);
        newObject.gameObject.SetActive(true);


    }

    public void DebugDeleteSave()
    {
        SaveHandler2.OrderDeleteFile();
    }

    public void DebugGainCoin()
    {
        PlayerHandler.instance.ChangeCoin(100);
    }
    public void DebugGainGem()
    {
        PlayerHandler.instance.ChangeGem(100);  
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