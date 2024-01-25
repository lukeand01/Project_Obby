using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageUnit : ButtonBase
{
    public StageData data {  get; private set; }
    MainMenuUI mainMenu;
    [SerializeField] TextMeshProUGUI stageNameText;
    [SerializeField] GameObject blocked;
    //and we should save some information in these things for the player to see.

    public void SetUpStage(MainMenuUI mainMenu, StageData stageData)
    {
        data = stageData;
        this.mainMenu = mainMenu;


        bool isAvailable = PlayerHandler.instance.stageProgress >= data.stageId - 2;
        blocked.SetActive(!isAvailable);


        UpdateUI();
    }

    void UpdateUI()
    {
        stageNameText.text = data.stageName;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        //select this fella.
        mainMenu.SelectStageUnit(this);
    }

    public void Select()
    {

    }
    public void Unselect()
    {

    }

}
