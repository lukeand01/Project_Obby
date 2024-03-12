using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerHandler : MonoBehaviour
{
    //GUIDE - this handles and holds all different player parts here.

    public static PlayerHandler instance;

    //public PlayerMovement movement { get; private set; }
    public PlayerMovement2 movement2 { get; private set; }


    public PlayerCamera cam { get; private set; }

    public PlayerController controller { get; private set; }

    public PlayerGraphic graphic { get; private set; }
    
    public PlayerSound sound { get; private set; }


    public Rigidbody rb { get; private set; }
    public BoxCollider boxCollider { get; private set; }


    //this defines what stage the player can palyer.
    public int stageProgress { get; private set; }
    public bool debugLiberateAllStages;




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        //movement = GetComponent<PlayerMovement>();
        movement2 = GetComponent<PlayerMovement2>();
        cam = GetComponent<PlayerCamera>();
        controller = GetComponent<PlayerController>();
        graphic = GetComponent<PlayerGraphic>();
        sound = GetComponent<PlayerSound>();

        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        currentHealth = debugInitialHealth;


        UIHandler.instance.uiPlayer.UpdateCoin(coins);
        UIHandler.instance.uiPlayer.UpdateLives(currentHealth);


        cam.ResetCam();
        //cam.ResetCamToIntroduction();
    }



    private void Update()
    {

    }

    public void ResetPlayer()
    {
        Debug.Log("rest player");

        ResetScenePlayer();
    }

    public void ResetScenePlayer()
    {
        cam.ResetCam();
        currentHealth = 3;
        UIHandler.instance.uiPlayer.UpdateLives(currentHealth);
        hasAlreadyWatchedAd = false;
        lastSpawnPoint = null;
        lastSpawnPointIndex = 0;
        ResetTempPowerList();
        RemoveIsDead();
        RemoveShield();

        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    #region STORE
    //each item is a 
    //if you do not have any item then we award the player two values. 

    void StartStore()
    {
        //this should only be called by the savehandler when there is no data.

        if(storeItensOwnedList == null)
        {
            storeItensOwnedList = new();
        }

        if(storeItensOwnedList.Count == 0)
        {
            int boyIndex = 2;
            int girlIndex = 4;
            int chickenDanceIndex = 5;

            AddStoreItem(boyIndex);
            AddStoreItem(girlIndex);
            AddStoreItem(chickenDanceIndex);

            //these are all teh fellas.
            //but now how we update teh fellas.

            StoreData graphicData = GameHandler.instance.storeHandler.GetStoreData(boyIndex);
            int graphicIndex =  (int)graphicData.GetGraphic().graphicType;
            graphic.SetGraphicIndex(graphicIndex);


            StoreData animationData = GameHandler.instance.storeHandler.GetStoreData(chickenDanceIndex);
            int animationIndex = (int)animationData.GetAnimation().animationType;
            graphic.SetAnimationIndex(animationIndex);
        }
    }
  
    public List<int> storeItensOwnedList  = new();

    public void AddStoreItem(int index)
    {
        storeItensOwnedList.Add(index);
        SaveHandler2.OrderToSaveData();
    }
    public bool HasStoreItem(int index)
    {
        if(storeItensOwnedList == null)
        {
            storeItensOwnedList = new List<int>();
        }


        foreach (var item in storeItensOwnedList)
        {
            if (item == index) return true;
        }
        return false;
    }

    //now how do we handle this with saved data?
    //i need this list just to add the power. because the grpahic and animation work without manually adding them
    public void SetNewItemOwnedList(List<int> storeItensList)
    {
        GameHandler.instance.storeHandler.AddAllPowersFromThisList(storeItensList);

        storeItensOwnedList = storeItensList;
    }


    #endregion

    //we save data everytime we load a new scene.


    #region SAVE
    public void UseSaveData(SaveClass save)
    {
        if (debugLiberateAllStages)
        {
            //then we get the all the stageprogression.
           stageProgress = GameHandler.instance.stageHandler.stageList.Count;
        }
        else
        {
            SetStageProgress(save.playerStageProgress);
        }
        

        graphic.SetAnimationIndex(save.playerCurrentAnimationIndex);
        graphic.SetGraphicIndex(save.playerCurrentGraphicIndex);

        storeItensOwnedList = save.playerItemsList;
        //and now we make the list of store.



    }



    public void UseEmptyData()
    {
        //just reset to the start.
        coins = initialGold;
        gems = initialGem;

        if (debugLiberateAllStages)
        {
            //then we get the all the stageprogression.
            stageProgress = GameHandler.instance.stageHandler.stageList.Count;
        }
        else
        {
            stageProgress = 1;
        }

        StartStore();

        foreach (var item in powerPermaList)
        {
            item.RemovePower();
        }
        powerPermaList.Clear();

    }

    #endregion

    #region DEBUG
    [ContextMenu("Debug Victory")]
    public void DebugVictory()
    {
        LocalHandler.instance.AddLocalCoin(3);
        PlayerWon();
    }


    [ContextMenu("Debug Gain 10 Coin")]
    public void DebugGainCoin()
    {
        ChangeCoin(10);
    }

    [ContextMenu("Debug Gain 10 Gems")]
    public void DebugGainGems()
    {
        ChangeGem(10);
    }

    [ContextMenu("Debug set stage progress to 5")]
    public void DebugStageProgress()
    {

        SetStageProgress(5);
        SaveHandler2.OrderToSaveData();
    }


    #endregion


    #region ECONOMY

    [SerializeField] int initialGold;

    public int coins { get; private set; }

    public void ChangeCoin(int amount)
    {
        coins += amount;
        UIHandler.instance.uiPlayer.UpdateCoin(coins, amount);
        UpdateMainMenuCurrency();
    }
    public void SetCoin(int amount)
    {
        coins = amount;
        UIHandler.instance.uiPlayer.UpdateCoin(coins);
        UpdateMainMenuCurrency();


    }
    public bool HasEnoughGold(int amount)
    {
        return coins >= amount;
    }

    [SerializeField] int initialGem;
    public int gems { get; private set; }

    public void ChangeGem(int amount)
    {
        gems += amount;
        UpdateMainMenuCurrency();
    }
    public void SetGem(int amount)
    {
        gems = amount;
        UpdateMainMenuCurrency();
    }
    public bool HasEnoughGem(int amount)
    {
        return gems >= amount;
    }


    void UpdateMainMenuCurrency()
    {

        if(MainMenuUI.instance != null)
        {
            MainMenuUI.instance.UpdatePlayerCurrencies();
        }
    }

    #endregion

    #region POWER

    //i need to send information here but how do i store it?
    //we have this list so we can remove it later.
    //but we need another list for perma power.

    //we need to send the list to the gamehandler and from there i retrieves everything the player needs.

    [SerializeField] List<PowerData> powerPermaList = new();

    public void AddPermaPower(PowerData newPower)
    {
        newPower.AddPower();
        powerPermaList.Add(newPower);
    }

    public bool HasPermaPower(PowerData data)
    {
        foreach (var item in powerPermaList)
        {
            if (item == data) return true;
        }
        return false;
    }



    List<PowerData> powerTempList = new();

    public void ResetTempPowerList()
    {
        foreach (var item in powerTempList)
        {
            item.RemovePower();
        }
    }

    public void AddTempPower(PowerData newPower)
    {
        newPower.AddPower();
        powerTempList.Add(newPower);
    }

    public bool HasTempPower(PowerData data)
    {
        foreach (var item in powerTempList)
        {
            if (item == data) return true;
        }
        return false;
    }

    #endregion

    #region DAMAGEABLE
    //WARNING - I MIGHT REMOVE THIS THING FROM THIS SCRIPT AND PUT INTO ANOTHER. FOR NOW THIS WORKS.
    bool isShielded;
    bool isShieldProcess;

    bool hasAlreadyWatchedAd;
    public bool isDead {  get; private set; }

    public int currentHealth {  get; private set; }
    [SerializeField] int debugInitialHealth;

    public void ReceiveShield()
    {
        isShielded = true;
    }
    public void RemoveShield()
    {
        isShielded = false;
    }

    //

    public void TakeDamage(bool notBlockable)
    {
        if (isDead)
        {
            return;
        }

        //replay the stage.
        if (isShielded && !notBlockable)
        {           
            return;
        }


        isDead = true;
        movement2.CompleteStopPlayer();
        UIHandler.instance.uiEnd.StartDefeat(currentHealth, hasAlreadyWatchedAd);
    }

    public void RespawnUsingHealth()
    {
        if (isDead)
        {
            currentHealth -= 1;
            UIHandler.instance.uiPlayer.UpdateLives(currentHealth);
            LocalHandler.instance.ResetScene();
        }
        else
        {
            Debug.LogError("Something wrong about respawn using health");
        }
       
    }

    public void RespawnUsingAd()
    {

        if (isDead)
        {
            hasAlreadyWatchedAd = true;
            LocalHandler.instance.ResetScene();
        }
        else
        {
            Debug.LogError("something wrong happened about respawn using ad");
        }

       
    }

    public void RespawnUsingNothing()
    {
        lastSpawnPointIndex = 0;
        LocalHandler.instance.ResetScene();
    }

    //i am using courotine because it was not working before.


    //the fella that call this is the sceneloader everytime it loads the game.
    //the respawn decisions are called from the button and they just change the values that will be changed later.

    public void OrderToEndGameFromTimer()
    {
        isDead = true;
        UIHandler.instance.uiEnd.StartDefeat(currentHealth, hasAlreadyWatchedAd);
    }



    public void RemoveIsDead()
    {
        isDead = false;

    }


    IEnumerator RemoveShieldProcess()
    {
        isShieldProcess = true;

        Debug.Log("triggered");

        //we count it as seconds.
        yield return new WaitForSeconds(0.5f);

        Debug.Log("end");

        isShielded = false;
        isShieldProcess = true;
    }

   
    #endregion

    #region RESPAWN

    [SerializeField] SpawnPoint lastSpawnPoint;
    public int lastSpawnPointIndex { get; private set;  }


    public void AssignNewSpawnPoint(SpawnPoint spawnPoint, int index)
    {
        if(lastSpawnPoint != null)
        {
            lastSpawnPoint.Unselect();
        }

        lastSpawnPoint = spawnPoint;
        lastSpawnPointIndex = index;

        Debug.Log("ASsign new spawn point with thsi index " + index);

    }

    [ContextMenu("Spawn In Last spawn")]
    public void SpawnInLastSpawnPoint()
    {

        if(lastSpawnPoint == null)
        {
            Debug.Log("has no last spawn");
            return;
        }
          
        //transform.position = lastSpawnPoint.transform.position;
        StartCoroutine(SpawnInLastPositionProcess());

    }

    public void DebugSpawnInLastPosition()
    {

        StartCoroutine(FixPlayerPositionProcess());
    }

    public void ForceRightRotationInRelationToSpawn()
    {
        lastSpawnPoint = LocalHandler.instance.GetRightSpawnPoint(lastSpawnPointIndex);
        Vector3 targetRotation = lastSpawnPoint.GetRotation();

        transform.rotation = Quaternion.Euler(targetRotation);  

    }

    public IEnumerator FixPlayerPositionProcess()
    {
        LocalHandler local = LocalHandler.instance;

        if (local == null)
        {
            Debug.LogError("local handler is null");
        }


        lastSpawnPoint = local.GetRightSpawnPoint(lastSpawnPointIndex);

        if (lastSpawnPoint == null)
        {
            Debug.Log("there was no one here");
            yield break;
        }


        Vector3 rightPos = lastSpawnPoint.GetSpawnPos();

        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = true;

        boxCollider.enabled = false;


        int progress = 0;

        

        while (transform.position != rightPos)
        {
            progress++;

            if (progress >= 10)
            {
                transform.position = Vector3.MoveTowards(transform.position, rightPos, 500);

            }
            else
            {
                transform.position = rightPos;
            }

            if (progress > 150)
            {
                Debug.Log("this was not working");
                yield break;
            }


            yield return new WaitForSeconds(0.01f);
        }

        

        Vector3 targetRotation = lastSpawnPoint.GetRotation();

        //now i have to try and rotate without also rotating the camera.
        cam.SetRotationX(targetRotation.y);


        boxCollider.enabled = true;
        rb.useGravity = true;
        rb.isKinematic = false;

    }
    //IM DOING THIS BECAUSE THE OTHER WAY WAS NOT WORKING.
    IEnumerator SpawnInLastPositionProcess()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        boxCollider.enabled = false;
        Vector3 target = lastSpawnPoint.GetSpawnPos();

        while (transform.position != target)
        {
            //transform.position = Vector3.MoveTowards(transform.position, target, 50);
            transform.position = target;
            yield return new WaitForSeconds(0.0001f);
        }

        

        boxCollider.enabled = true; 
        rb.useGravity = true;
    }




    #endregion


    #region DIE FROM FALL
    bool isDieFromFallProcess;
    public void DieFromFall()
    {
        //stop control
        //control camera to look
        //and wait a bit to kill it.

        if (isDieFromFallProcess)
        {
            Debug.Log("cannot keep repeating");
            return;
        }


        StartCoroutine(DieFromFallProcess());
    }


    IEnumerator DieFromFallProcess()
    {
        LocalHandler.instance.StopTimer();


        isDieFromFallProcess = true;
        controller.blockClass.AddBlock("FallDeath", BlockClass.BlockType.Complete);
        cam.MakeCamWatchFallDeath();
        graphic.PlayFallAnimation();
        GameHandler.instance.soundHandler.CreateSFX(sound.fallClip);

        rb.velocity = new Vector3(0, rb.velocity.y, 0);

        //Time.timeScale = 0.3f;

        yield return new WaitForSeconds(1.5f);

        //then call death ui
        TakeDamage(true);
        isDieFromFallProcess = false;
        controller.blockClass.RemoveBlock("FallDeath");

    }


    #endregion

    #region STAGE

    public void ChangeProgress(int modifier = 0)
    {
        int indexOfLastStage = LocalHandler.instance.data.stageId;


        if (indexOfLastStage > stageProgress)
        {
            stageProgress = indexOfLastStage + modifier;
        }

    }

    public void SetStageProgress(int value)
    {



        stageProgress = value;

        MainMenuUI mainMenuUI = MainMenuUI.instance;

        if(mainMenuUI != null)
        {
            mainMenuUI.playUI.UpdateStageUnits();
        }
    }


    #endregion

    #region WIN ANIMATION

    public void PlayerWon()
    {
        movement2.CompleteStopPlayer();
        LocalHandler.instance.StopTimer();
        UIHandler.instance.ControlInputButtons(false);
        //i also want to hide everything else.
        graphic.PlayVictoryAnimation();
        float timer = 1.5f;
        StartCoroutine(cam.RotateCameraForDanceProcess(timer));
        UIHandler.instance.uiEnd.StartVictory();
    }

  


    #endregion

    #region LOGIC SO PLAYER CAN USE MOVE TERRAIN 
    TerrainMoveBehavior currentMoveTerrain;

    public void MakeParent(TerrainMoveBehavior terrainMove)
    {
        currentMoveTerrain = terrainMove;
        transform.parent = currentMoveTerrain.transform;
    }

    public void CancelParent(TerrainMoveBehavior moveTerrain)
    {
        if(currentMoveTerrain != null)
        {
            if (moveTerrain.id == currentMoveTerrain.id)
            {
                currentMoveTerrain = null;
                transform.parent = null;
            }
        }

        
    }

    #endregion


    #region TIME POWER
    public float TimerModifier { get; private set; } = 1;

    public void AddTimerModifier()
    {
        TimerModifier = 0.7f;
    }
    public void RemoveTimerModifier()
    {
        TimerModifier = 1;
    }


    #endregion

}


