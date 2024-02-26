using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoreAnimationUnit : StoreBaseUnit
{
    //it does the same thing but for animation

    StoreAnimationData data;
    StoreUI handler;

    [SerializeField] TextMeshProUGUI danceNameText;
    [SerializeField] TextMeshProUGUI dancePriceText;
    [SerializeField] GameObject gemSimbol;
    [SerializeField] GameObject coinSimbol;
    [SerializeField] GameObject priceHolder;
    [SerializeField] TextMeshProUGUI stateText;
    [SerializeField] GameObject selected;

    bool isSelected;
    bool isAlreadyOwned;
    bool isCurrentlyUsing;

    public void SetUp(StoreAnimationData data, StoreUI handler)
    {
        this.data = data;
        this.handler = handler;

        UpdateUI();
    }


    public void UpdateOwnership()
    {
        isAlreadyOwned = PlayerHandler.instance.HasStoreItem(data.storeIndex);
        isCurrentlyUsing = PlayerHandler.instance.graphic.animationIndex == (int)data.animationType;

        priceHolder.SetActive(!isAlreadyOwned);
        stateText.gameObject.SetActive(isAlreadyOwned);

        if (isCurrentlyUsing)
        {
            stateText.text = "Using";
        }
        else
        {
            stateText.text = "Bought";
        }
    }
    void UpdateUI()
    {
        danceNameText.text = data.storeItemName;
        dancePriceText.text = data.storePrice.ToString();

        gemSimbol.SetActive(data.currencyType == CurrencyType.Gem);
        coinSimbol.SetActive(data.currencyType == CurrencyType.Gold);
    }


    public void Select()
    {
        isSelected = true;
        float time = 0.15f;
        selected.transform.DOKill();
        selected.transform.DOScale(new Vector3(2.1f, 1.12f, 0), time);
    }
    public void UnSelect()
    {
        isSelected = false;
        float time = 0.15f;
        selected.transform.DOKill();
        selected.transform.DOScale(0, time);
    }

    void ChangeAnimation()
    {

    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (isSelected)
        {
            if (isAlreadyOwned)
            {
                //then we start using this thing.

                ChangeAnimation();
            }
            else
            {
                //then we ask for confirmation to buy it.
                handler.StartBuyItem(data, this);
            }
        }
        else
        {
            //then we select it.

            handler.SelectAnimation(this);
        }
    }

    public int GetAnimationIndex()
    {
        return (int)data.animationType;
    }

}
