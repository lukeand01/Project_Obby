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

    [SerializeField] List<PowerData> allAvailablePowerList = new();



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

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UIHandler.instance.CreatePowerButtons(allAvailablePowerList);
    }

    public void CreateSFX(AudioClip clip)
    {
        
    }
}
