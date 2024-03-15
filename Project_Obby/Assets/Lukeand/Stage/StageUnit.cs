using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageUnit : ButtonBase
{
    
    public StageData data {  get; private set; }
    PlayUI handler;

    [Separator("STAGE UNIT")]
    [SerializeField] TextMeshProUGUI stageNameText;
    [SerializeField] GameObject blocked;
    [SerializeField] GameObject playIcon;
    [SerializeField] Transform coreHolder;
    [SerializeField] GameObject starHolder;
    [SerializeField] Image[] stars;

    [Separator("INFO HOLDER")]
    [SerializeField] Transform infoHolder;
    [SerializeField] TextMeshProUGUI timeStageText;
    [SerializeField] TextMeshProUGUI timeBestText;

    [Separator("GOLD")]
    [SerializeField] Transform coinHolder;
    [SerializeField] TextMeshProUGUI coinText;


    //and we should save some information in these things for the player to see.

    Vector3 coreOriginalPos;
    Vector3 infoOriginalPos;
    Vector3 coinOriginalPos;

    bool isAvailable;

    private void Start()
    {
        
    }

    public void SetUpStage(PlayUI handler, StageData stageData, int playerStageProgress)
    {
        coreOriginalPos = coreHolder.localPosition;
        infoOriginalPos = infoHolder.localPosition;
        coinOriginalPos = coinHolder.localPosition;

        playIcon.SetActive(false);

        data = stageData;
        this.handler = handler;



        UpdateAvailabilityUI(playerStageProgress);
        UpdateStar();
        UpdateUI();
    }

    public void UpdateAvailabilityUI(int playerStageProgress)
    {
        bool isAvailable = playerStageProgress >= data.stageId - 2;
        blocked.SetActive(!isAvailable);
        coinHolder.gameObject.SetActive(isAvailable);
        stageNameText.gameObject.SetActive(isAvailable);
        ControlStarHolderVisibility(isAvailable);

        this.isAvailable = isAvailable;
    }

    void UpdateStarsGained()
    {
        int currentStarsGained = data.stageStarGained;

        for (int i = 0; i < currentStarsGained; i++)
        {
            //we paint them

        }

    }

    void UpdateUI()
    {
        stageNameText.text = data.stageName;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        //select this fella.

        if (playIcon.activeInHierarchy)
        {
            // this means that you can do this;
            handler.PlayCurrentStage();
            return;
        }


        if(isAvailable)
        {
            //it becomes selected.
            handler.SelectStageUnit(this);
        }
        else
        {
            //it wiggles rejecting it.         
            Reject();
            handler.CancelStageUnit();
        }

        //mainMenu.SelectStageUnit(this);
        
        
    }

    //always fix rotation because of isselected 

    #region SELECT 

    public void Select()
    {
        stageNameText.gameObject.SetActive(false);
        playIcon.gameObject.SetActive(true);

        StopAllCoroutines();
        ResetUnit();
        StartCoroutine(SelectProcess());
    }
    public void Unselect()
    {
        stageNameText.gameObject.SetActive(true);
        playIcon.gameObject.SetActive(false);


        StopAllCoroutines();
        ResetUnit();
        StartCoroutine(UnSelectProcess());
    }

    void Reject()
    {
        StopAllCoroutines();
        ResetUnit();
        StartCoroutine(RejectProcess());
    }

    void ResetUnit()
    {
        //reset important things.
        coreHolder.DOKill();
        coreHolder.localRotation = Quaternion.Euler(0,0,0);
        coreHolder.localPosition = coreOriginalPos;
    }

    #endregion

    #region ANIMATION PROCESS

    IEnumerator SelectProcess()
    {
        float timer = 0.3f;
        transform.DOScale(1.2f, timer);

        infoHolder.DOLocalMove(infoOriginalPos + new Vector3(0, -50, 0), timer);
        coinHolder.DOLocalMove(coinOriginalPos + new Vector3(0, -35, 0), timer);

        yield return new WaitForSeconds(timer);

        StartCoroutine(IsSelectedProcess());
    }
    IEnumerator UnSelectProcess()
    {
        float timer = 0.3f;
        transform.DOScale(1f, timer);

        infoHolder.DOLocalMove(infoOriginalPos, timer);
        coinHolder.DOLocalMove(coinOriginalPos, timer);

        yield return new WaitForSeconds(timer);
    }

    IEnumerator IsSelectedProcess()
    {
        //it keeps moving till being cancelled.
        float timer = 0.6f;

        coreHolder.DORotate(new Vector3(0, 0, -5), timer);

        yield return new WaitForSeconds(timer);

        coreHolder.DORotate(new Vector3(0, 0, 5), timer);

        yield return new WaitForSeconds(timer);

        StartCoroutine(IsSelectedProcess());
    }

    IEnumerator RejectProcess()
    {
        int cicles = 30;
        float timer = 0.01f;
        float intensity = 3.5f;

        for (int i = 0; i < cicles; i++)
        {

            float x = Random.Range(-intensity, intensity);
            coreHolder.localPosition = coreOriginalPos + new Vector3(x, 0, 0);
            yield return new WaitForSeconds(timer);
            
        }

        coreHolder.localPosition = coreOriginalPos;

    }

    #endregion

    #region STARS

    void ControlStarHolderVisibility(bool isVisible)
    {
        starHolder.SetActive(isVisible);


        if (isVisible)
        {
            UpdateStar();
        }
    }

    void UpdateStar()
    {
        //tell what star
        Color fullColor = Color.white;
        Color emptyColor = Color.black;

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].color = emptyColor;
        }

        for (int i = 0; i < data.stageStarGained; i++)
        {
           
            stars[i].color = fullColor;
        }

        if(data.stageStarGained > 0)
        {
            Debug.Log("this has stars");
        }

        Debug.Log("this was visible");
    }



    #endregion


}


//things i want to show in this thing. 
//select animation
//selected animation
//stairs and stage level.
//unlock and locked states.