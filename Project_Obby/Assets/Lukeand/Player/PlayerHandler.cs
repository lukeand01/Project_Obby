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

    public void DebugStageProgress()
    {
        stageProgress +=1;
        SaveHandler2.OrderToSaveData();
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
        gold = initialGold;

        UIHandler.instance.uiPlayer.UpdateGold(gold);
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
        ResetPowerList();
        RemoveIsDead();
        RemoveShield();

    }

    #region STORE
    //each item is a 

    public List<int> storeItensOwnedList { get; private set; } = new();

    public void AddStoreItem(int index)
    {
        storeItensOwnedList.Add(index);
    }
    public bool HasStoreItem(int index)
    {
        foreach (var item in storeItensOwnedList)
        {
            if (item == index) return true;
        }
        return false;
    }


    #endregion



    #region SAVE
    public void UseSaveData(SaveClass save)
    {
        SetStageProgress(save.playerStageProgress);

        graphic.SetAnimationIndex(save.playerCurrentAnimationIndex);
        graphic.SetGraphicIndex(save.playerCurrentGraphicIndex);

        storeItensOwnedList = save.playerItemsList;
        //and now we make the list of store.



    }



    public void UseEmptyData()
    {
        //just reset to the start.


    }

    #endregion

    #region DEBUG
    [ContextMenu("Debug Victory")]
    public void DebugVictory()
    {
        LocalHandler.instance.AddLocalCoin(3);
        PlayerWon();
    }
    [ContextMenu("Debug Change Graphic")]
    public void DebugChangeGraphic()
    {
        graphic.animationIndex = 3;
        SaveHandler2.OrderToSaveData();
    }
    [ContextMenu("Debug Change Animation")]
    public void DebugChangeAnimation()
    {
        graphic.graphicIndex = 3;
        SaveHandler2.OrderToSaveData();
    }

    #endregion


    #region ECONOMY

    [SerializeField] int initialGold;
    public int gold { get; private set; }

    public void ChangeGold(int amount)
    {
        gold += amount;
        UIHandler.instance.uiPlayer.UpdateGold(gold, amount);
    }
    public void SetGold(int amount)
    {
        gold = amount;
        UIHandler.instance.uiPlayer.UpdateGold(gold);
    }
    public bool HasEnoughGold(int amount)
    {
        return gold >= amount;
    }


    public int gems { get; private set; }

    public void ChangeGem(int amount)
    {
        gems += amount;
    }
    public void SetGem(int amount)
    {
        gems = amount;
    }
    public bool HasEnoughGem(int amount)
    {
        return gems >= amount;
    }

    #endregion

    #region POWER

    List<PowerData> powerList = new();

    public void ResetPowerList()
    {
        foreach (var item in powerList)
        {
            item.RemovePower();
        }
    }

    public void AddPower(PowerData newPower)
    {
        powerList.Add(newPower);
    }

    public bool HasPower(PowerData data)
    {
        foreach (var item in powerList)
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

        UIHandler.instance.uiEnd.StartDefeat(currentHealth, hasAlreadyWatchedAd);
    }

    public void RespawnUsingHealth()
    {
        currentHealth -= 1;
        UIHandler.instance.uiPlayer.UpdateLives(currentHealth);
        LocalHandler.instance.ResetScene();
    }

    public void RespawnUsingAd()
    {
        hasAlreadyWatchedAd = true;
        LocalHandler.instance.ResetScene();
    }

    public void RespawnUsingNothing()
    {
        lastSpawnPointIndex = 0;
        LocalHandler.instance.ResetScene();
    }

    //i am using courotine because it was not working before.


    //the fella that call this is the sceneloader everytime it loads the game.
    //the respawn decisions are called from the button and they just change the values that will be changed later.

    



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




}


