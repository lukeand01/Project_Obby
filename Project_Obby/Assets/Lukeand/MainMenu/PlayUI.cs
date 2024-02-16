using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayUI : MonoBehaviour
{


    [Separator("STAGE")]
    [SerializeField] StageUnit stageTemplate;
    [SerializeField] Transform stageContainer;

    List<StageUnit> allStageUnitList = new();
   

    #region HANDLE STAGES


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

    //we are going to get that information and put everything into order.


}
