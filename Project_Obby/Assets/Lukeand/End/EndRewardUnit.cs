using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EndRewardUnit : ButtonBase
{
    //this will  do the actual effect.
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] Transform fadePos;
    [SerializeField] GameObject adIcon;
    [SerializeField] GameObject adHolder;
    [SerializeField] GameObject bonusIcon;
    [SerializeField] bool isAd;
    [SerializeField] bool isCoin;



    int valueStored = 0;

    bool isUsed;

    int valueTotal;
    int valueCurrent;


    public void Add(int newValue)
    {
        valueTotal += newValue;
        StopAllCoroutines();
        StartCoroutine(CountRewardProcess());
    }

    IEnumerator CountRewardProcess()
    {
        transform.DOScale(0.65f, 0.2f);

        while (valueTotal > valueCurrent)
        {
            valueCurrent += 1;
            coinText.text = valueCurrent.ToString();
            yield return new WaitForSeconds(0.2f);
        }

        transform.DOScale(0.5f, 0.2f);

    }


    public void CallFadeUI(string value)
    {
        FadeUI newObject = Instantiate(UIHandler.instance.uiFade);
        newObject.transform.SetParent(fadePos);
        newObject.transform.localPosition = Vector3.zero;
        newObject.SetUp(value, Color.blue);
    }

    

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (isUsed) return;
        if (!isAd) return;

        isUsed = true;

        if (isCoin)
        {
            //gain coin
            GameHandler.instance.adHandler.RequestRewardAd(RewardType.ModifyCoinValue);
        }
        else
        {
            //gain gme.
        }


    }

}
