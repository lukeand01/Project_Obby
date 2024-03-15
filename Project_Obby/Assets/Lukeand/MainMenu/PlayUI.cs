using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{


    [Separator("STAGE 2")]
    [SerializeField] StageUnit2 stageTemplate2;
    [SerializeField] Image emptyStageTemplate;
    [SerializeField] Transform stageContainer2;

    List<StageUnit2> allStageUnitList2 = new List<StageUnit2>();

    [Separator("STAGE")]
    [SerializeField] StageUnit stageTemplate;
    [SerializeField] Transform stageContainer;


    List<StageUnit> allStageUnitList = new();


    private void Start()
    {
        
    }


    #region HANDLE STAGES

    //the major difference is that will be increasing the screen based in the number.
    //also i will be making connections.

    public void CreateStageUnits(List<StageData> stageList)
    {
        //create for some effect.   
        int playerProgress = PlayerHandler.instance.stageProgress;

        foreach (StageData stageData in stageList)
        {
            StageUnit newObject = Instantiate(stageTemplate, Vector2.zero, Quaternion.identity);
            newObject.SetUpStage(this, stageData, playerProgress);
            newObject.transform.parent = stageContainer.transform;
            allStageUnitList.Add(newObject);  

        }
    }

   





    public void UpdateStageUnits()
    {
        int playerProgress = PlayerHandler.instance.stageProgress;


        foreach (var item in allStageUnitList)
        {
            item.UpdateAvailabilityUI(playerProgress);
        }

        foreach (var item in allStageUnitList2)
        {

        }

    }

    StageUnit currentStageUnit;
    public void SelectStageUnit(StageUnit stageUnit)
    {

        if (currentStageUnit != null)
        {
            currentStageUnit.Unselect();
        }

        currentStageUnit = stageUnit;
        currentStageUnit.Select();
        //DescribeStage();
    }

    public void CancelStageUnit()
    {
        if (currentStageUnit != null)
        {
            currentStageUnit.Unselect();
            currentStageUnit = null;
        }
    }


    

    public void PlayCurrentStage()
    {
        if (currentStageUnit == null)
        {
            Debug.Log("this was null");
            return;
        }

        //load this stage.
        GameHandler.instance.sceneLoader.ChangeScene(currentStageUnit.data);
    }


    #endregion


    #region HANDLE STAGES 2
    public void CreateStageUnits2(List<StageData> stageList)
    {




        int playerProgress = PlayerHandler.instance.stageProgress;

        for (int i = 0; i < stageList.Count; i++)
        {
            StageUnit2 newObject = Instantiate(stageTemplate2, Vector2.zero, Quaternion.identity);
            //newObject.SetUpStage(this, stageData, playerProgress);
            newObject.SetUp(stageList[i], this, playerProgress, GetPathOrder(i, stageList.Count));
            newObject.transform.parent = stageContainer2.transform;
            allStageUnitList2.Add(newObject);

            newObject.transform.localScale = Vector2.one;
        }

        for (int i = 0; i < 5; i++)
        {
            Image newObject = Instantiate(emptyStageTemplate, Vector2.zero, Quaternion.identity);
            newObject.transform.SetParent(stageContainer2);
        }

        foreach (var item in allStageUnitList2)
        {
            item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y, 0);
        }


    }

    StageUnit2 currentStageUnit2;
    public void SelectStageUnit2(StageUnit2 stageUnit)
    {

        if (currentStageUnit2 != null)
        {
            currentStageUnit2.Unselect();
        }
       

        currentStageUnit2 = stageUnit;
        currentStageUnit2.Select();

    }

    public void CancelStageUnit2()
    {

        if (currentStageUnit2 != null)
        {
            currentStageUnit2.Unselect();
            currentStageUnit2 = null;
        }
    }


    public void PlayCurrentStage2()
    {
        if (currentStageUnit2 == null)
        {
            Debug.Log("this was null");
            return;
        }

        //load this stage.
        GameHandler.instance.sceneLoader.ChangeScene(currentStageUnit2.data);
    }




    List<int> GetPathOrder(int current, int total)
    {
        List<int> newPathList = new();

        if (current > 0)
        {
            newPathList.Add(0);
        }

        if (current <= total - 2)
        {
            newPathList.Add(1);
        }

        return newPathList;
    }

    #endregion


    //we are going to get that information and put everything into order.


}
