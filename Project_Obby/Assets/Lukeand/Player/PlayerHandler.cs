using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    //GUIDE - this handles and holds all different player parts here.

    public static PlayerHandler instance;

    public PlayerMovement movement { get; private set; }
    public PlayerCamera cam { get; private set; }

    public PlayerController controller { get; private set; }

    public Rigidbody rb { get; private set; }
    public BoxCollider boxCollider { get; private set; }


    //this defines what stage the player can palyer.
    public int stageProgress { get; private set; }

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


        movement = GetComponent<PlayerMovement>();
        cam = GetComponent<PlayerCamera>();
        controller = GetComponent<PlayerController>();

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


    }

    public void ResetPlayer()
    {
        Debug.Log("rest player");

        ResetScenePlayer();
    }

    public void ResetScenePlayer()
    {
        currentHealth = 3;
        UIHandler.instance.uiPlayer.UpdateLives(currentHealth);
        hasAlreadyWatchedAd = false;
        lastSpawnPoint = null;
        lastSpawnPointIndex = 0;
        ResetPowerList();
        RemoveIsDead();
        RemoveShield();
    }

    


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


    public int diamond { get; private set; }

    public void ChangeDiamond(int amount)
    {
        diamond += amount;
    }
    public void SetDiamond(int amount)
    {
        diamond = amount;
    }
    public bool HasEnoughDiamond(int amount)
    {
        return diamond >= amount;
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

    public IEnumerator FixPlayerPositionProcess()
    {
        LocalHandler local = LocalHandler.instance;

        if(local == null)
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

            if(progress >= 10)
            {
                transform.position = Vector3.MoveTowards(transform.position, rightPos, 500);

            }else
            {
                transform.position = rightPos;
            }

            if(progress > 150)
            {
                Debug.Log("this was not working");
                yield break;
            }

            
            yield return new WaitForSeconds(0.01f);
        }



        boxCollider.enabled = true;
        rb.useGravity = true;
        rb.isKinematic = false;

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

        Debug.Log("ASsign new spawn point " + Random.Range(0,100));

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
        isDieFromFallProcess = true;
        controller.blockClass.AddBlock("FallDeath", BlockClass.BlockType.Complete);
        cam.MakeCamWatchFallDeath();

        rb.velocity = new Vector3(0, rb.velocity.y, 0);

        //Time.timeScale = 0.3f;

        yield return new WaitForSeconds(1.5f);

        //then call death ui
        TakeDamage(true);
     
    }

    public void ChangeProgress(int modifier = 0)
    {
        int indexOfLastStage = LocalHandler.instance.data.stageId;


        if (indexOfLastStage > stageProgress)
        {
            stageProgress = indexOfLastStage + modifier;
        }

    }

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
}


