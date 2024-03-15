using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PresentationUI : MonoBehaviour
{
    GameObject holder;

    [SerializeField] Image[] stars;
    [SerializeField] TextMeshProUGUI timerBestText;
    [SerializeField] TextMeshProUGUI timeTotalText;
    [SerializeField] Image stageImage;
    [SerializeField] TextMeshProUGUI stageText;
    [SerializeField] Image barImage;
    [SerializeField] Image starInfoBackground;
    //i will need to animate this parts to make it look better.

    bool inProcess;


    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    public void StartPresentationUI(StageData data)
    {
        holder.SetActive(true);

        Color fullColor = Color.white;
        Color emptyColor = Color.black;

        for (int i = 0; i < stars.Length; i++)
        {
            if(data.stageStarGained > i)
            {
                stars[i].color = fullColor;
            }
            else
            {
                stars[i].color = emptyColor;
            }
        }

        string additiveZeroSecond2 = "";
        string additiveZeroMinute2 = "";

        if (data.stageLimitTimer.minutes > 10)
        {
            additiveZeroMinute2 = "0";
        }

        if (data.stageLimitTimer.seconds > 10)
        {
            additiveZeroSecond2 = "0";
        }


        timeTotalText.text = $"{additiveZeroMinute2}{data.stageLimitTimer.minutes}:{additiveZeroSecond2}{data.stageLimitTimer.seconds}";

        if(data.stageCompletedTimer != null)
        {
            string additiveZeroSecond = "";
            string additiveZeroMinute = "";

            if(data.stageCompletedTimer.minutes > 10)
            {
                additiveZeroMinute = "0";
            }

            if(data.stageCompletedTimer.seconds > 10)
            {
                additiveZeroSecond = "0";
            }


            timerBestText.text = $"{additiveZeroMinute}{data.stageCompletedTimer.minutes}:{additiveZeroSecond}{data.stageCompletedTimer.seconds}";
        }
        else
        {
            timerBestText.text = "Not Completed!";
        }


        stageImage.color = data.stageColor;
        barImage.color = data.stageColor;
        starInfoBackground.color = data.stageColor;

        stageText.text = $"{data.stageName} - {data.stageWorld}";
    }


    public void StartStage()
    {
        //we close this. we call the presentation camera.
        //we also need to remove this fella.

        //we first need to remove this then we call it.



        if (inProcess) return;


        Debug.Log("called ");

        inProcess = true;
        StopAllCoroutines();
        StartCoroutine(CloseStagePresentationUI());      
    }

    //
    IEnumerator CloseStagePresentationUI()
    {

        holder.SetActive(false);

        LocalHandler.instance.CallStartStage();

        yield return null;
    }

}
