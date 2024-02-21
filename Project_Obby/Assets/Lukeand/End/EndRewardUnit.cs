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
    public void SetUp(int value)
    {
        coinText.text = value.ToString();
        valueStored = value;


        if (isAd)
        {
            //then we are going to start shaking the thing.
        }
    }

    public void Add()
    {



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
