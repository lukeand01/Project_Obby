using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalHandler : MonoBehaviour
{
    public static LocalHandler instance;


    public StageData data {  get; private set; }
    [SerializeField] List<SpawnPoint> spawnPointList = new();
    [SerializeField] StageData debugStart;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (debugStart)
        {
            StartLocalHandler(debugStart);
            //i will call to fix the playerposition here just for testing.
            PlayerHandler.instance.DebugSpawnInLastPosition();


            //i need to start the counter. i also need 

        }

        

    }


    //localhandler will just hold the list and the playter will use it.

    public void StartLocalHandler(StageData stage)
    {
        data = stage;

        for (int i = 0; i < spawnPointList.Count; i++)
        {
            spawnPointList[i].SetIndex(i);
        }       
    }
   
    public SpawnPoint GetRightSpawnPoint(int index)
    {
        if(index > spawnPointList.Count)
        {
            Debug.Log("there was a problem here");
            return null;
        }

        RemoveAllSpawnsPriorToTheCurrentOne(index);
        spawnPointList[index].MakeItUsed();
        return spawnPointList[index];
    }


    void RemoveAllSpawnsPriorToTheCurrentOne(int index)
    {
        for (int i = 0; i < index; i++)
        {
            spawnPointList[i].MakeItUsed();
        }
    }


    public void ResetScene()
    {
        GameHandler.instance.sceneLoader.ResetScene(data);
    }
}
