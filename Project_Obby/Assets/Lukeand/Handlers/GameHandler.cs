using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    //GUIDE - this holds all handler references. anything will come through here to go to a especific handler
    //OBSERVATION - the only handler not attached to this is the uihandler.

    public static GameHandler instance;


    public StageHandler stageHandler {  get; private set; }
    public SceneLoader sceneLoader { get; private set; }
    public AdHandler adHandler { get; private set; }
    public GraphicalHandler graphicalHandler { get; private set; }

    public SoundHandler soundHandler { get; private set; }

    public RewardHandler rewardHandler { get; private set; }

    public StoreHandler storeHandler { get; private set; }  



    [SerializeField] List<PowerData> allAvailablePowerList = new();



    public void PauseTimeScale()
    {
        Time.timeScale = 0;
    }

    public void ResumeTimeScale()
    {
        Time.timeScale = 1;
    }


    private void Update()
    {
        

    }
    private void Awake()
    {
      
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        stageHandler = GetComponent<StageHandler>();
        sceneLoader = GetComponent<SceneLoader>();
        adHandler = GetComponent<AdHandler>();
        graphicalHandler = GetComponent<GraphicalHandler>();
        soundHandler = GetComponent<SoundHandler>();
        rewardHandler = GetComponent<RewardHandler>();
        storeHandler = GetComponent<StoreHandler>();
        //SaveHandler2.DeleteData("0");




        DontDestroyOnLoad(gameObject);
    }



    [ContextMenu("DEBUG SAVE DATA")]
    public void DebugSaveData()
    {
        SaveHandler2.OrderToSaveData();
    }



    private void Start()
    {
        UIHandler.instance.CreatePowerButtons(allAvailablePowerList);
        SaveHandler2.OrderToLoadData();


    }

   
}
