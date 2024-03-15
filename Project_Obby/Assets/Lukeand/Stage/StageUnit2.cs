using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageUnit2 : ButtonBase
{
    //This will be a new one.

    //FEATURES:
    //has 3 different frames: unlocked, locked and fullycompleted.
    //it has stars in the bottom
    //it has a name
    //it has connections with the next. so if its not the last or the first it will have a connection
    //those connections will shine a different color if the stage has been unlocked.


    public StageData data {  get; private set; }
    PlayUI uiHandler;

    bool isUnlocked = false;
    bool isSelected = false;

    [Separator("FRAME REFERENCES")]
    [SerializeField] GameObject frameHolder;
    [SerializeField] Image frameLocked;
    [SerializeField] Image frameUnlocked;
    [SerializeField] Image frameFullyCompleted;

    [Separator("INSIDE FRAME REFERENCES")]
    [SerializeField] TextMeshProUGUI stageNameText;
    [SerializeField] Image[] stars;


    [Separator("PATH REFERENCES")]
    [SerializeField] Image[] paths; //0 is right. 1 is down. 2 is left. 3 is up.
    Color pathlockedColor;
    Color pathUnlockedColor;

    [Separator("PLAY ICON REFERENCES")]
    [SerializeField] Image playIconBlue;
    [SerializeField] Image playIconYellow;


    [Separator("ESPECIAL EFFECT REFERENCES")]
    [SerializeField] Image isSelectedImage;

    public void SetUp(StageData data, PlayUI uiHandler, int playerProgress, List<int> pathOrderList)
    {

        this.data = data;
        this.uiHandler = uiHandler;

        UpdateFrame(playerProgress);
        UpdateInsideFrame();
        UpdatePath(pathOrderList, playerProgress);
        UpdatePlayIcon();
        UpdateStars();

    }


    void UpdateFrame(int playerProgress)
    {
        bool isAvailable = playerProgress >= data.stageId - 2 || PlayerHandler.instance.debugLiberateAllStages;

        isUnlocked = isAvailable;

        frameLocked.gameObject.SetActive(!isAvailable);
        frameUnlocked.gameObject.SetActive(isAvailable);

        bool hasAllStars = isAvailable && data.stageStarGained >= 3;

        frameFullyCompleted.gameObject.SetActive(hasAllStars);


    }

    void UpdateInsideFrame()
    {
        stageNameText.text = data.stageName;

        for (int i = 0; i < stars.Length; i++)
        {
            if (data.stageStarGained > i)
            {
                stars[i].color = Color.white;
            }
            else
            {
                stars[i].color = Color.black;
            }


            
        }
    }

    void UpdatePath(List<int> pathOrder, int playerProgress)
    {

        //i
        bool isAvailable = playerProgress >= data.stageId - 2;
        bool isLastAvailable = playerProgress  >= data.stageId - 3; 


        List<Image> pathAllowedList = new();

        foreach (var item in paths)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in pathOrder)
        {
            paths[item].gameObject.SetActive(true); 
            pathAllowedList.Add(paths[item]);
        }

        if(isAvailable)
        {
            paths[1].color = Color.black;
        }
        else
        {
            paths[1].color = Color.white;
        }
        

        if(isLastAvailable)
        {
            paths[0].color = Color.black;
        }
        else
        {
            paths[0].color = Color.white;
        }

    }

    void UpdatePlayIcon()
    {
        //only when you click on this does it appear.
        bool fullStar = data.stageStarGained >= 3;

        playIconBlue.gameObject.SetActive(isSelected && !fullStar);
        playIconYellow.gameObject.SetActive(isSelected && fullStar);

        stageNameText.gameObject.SetActive(!isSelected);
        
    }

    void UpdateStars()
    {


        for (int i = 0; i < stars.Length; i++)
        {
            if(data.stageStarGained > i)
            {
                stars[i].color = Color.white; 
            }
            else
            {
                stars[i].color = Color.black;
            }
        }
    }


    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);


        if (!isUnlocked)
        {
            return;
        }

        if (isSelected)
        {
            uiHandler.PlayCurrentStage2();
        }
        else
        {
            uiHandler.SelectStageUnit2(this);
        }

        isSelected = !isSelected;
        UpdatePlayIcon();
    }

    public void Select()
    {
          
        StopAllCoroutines();
        StartCoroutine(StartIsSelectedProcess());

    }
    public void Unselect()
    {

        isSelected = false;
        UpdatePlayIcon();
        StopAllCoroutines();
        StartCoroutine(StopIsSelectedProcess());

    }



    IEnumerator StartIsSelectedProcess()
    {
        //instead of just bringing 
        float scaleDuration = 0.25f;
        isSelectedImage.gameObject.SetActive(true);
        frameHolder.transform.DOScale(1.3f, scaleDuration);

        float timer = 0.01f;

        while(isSelectedImage.color.a < 1)
        {
            var a = isSelectedImage.color;
            a.a += 0.01f;
            isSelectedImage.color = a;
            yield return new WaitForSeconds(timer);
        }

        StartCoroutine(IsSelectedProcess());
    }

    IEnumerator IsSelectedProcess()
    {


        float timer = 0.01f;
        float changeValue = 0.008f;
        while(isSelectedImage.color.a > 0.3f)
        {
            var a = isSelectedImage.color;
            a.a -= changeValue;
            isSelectedImage.color = a;
            yield return new WaitForSeconds(timer);
        }


        while(isSelectedImage.color.a < 1f)
        {
            var a = isSelectedImage.color;
            a.a += changeValue;
            isSelectedImage.color = a;
            yield return new WaitForSeconds(timer);
        }

        StartCoroutine (IsSelectedProcess());
    }

    IEnumerator StopIsSelectedProcess()
    {
        float timer = 0.01f;

        float scaleDuration = 0.25f;
        frameHolder.transform.DOScale(1f, scaleDuration);


        while (isSelectedImage.color.a > 0)
        {
            var a = isSelectedImage.color;
            a.a -= 0.01f;
            isSelectedImage.color = a;
            yield return new WaitForSeconds(timer);
        }

        isSelectedImage.gameObject.SetActive(false);

    }
}
