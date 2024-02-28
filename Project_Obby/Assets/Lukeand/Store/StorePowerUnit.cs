using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StorePowerUnit : StoreBaseUnit
{
    //whats the difference?
    //how to confirm a purchase? it needs all to be done through a confirmation window.

    //the wya it works is that you se4lected. and if you click again you receive a selection windows.

    StorePowerData data;
    StoreUI handler;

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image icon;
    [SerializeField] GameObject gemSymbol;
    [SerializeField] GameObject goldSymbol;
    [SerializeField] TextMeshProUGUI valueText;
    [SerializeField] Transform selected;
    [SerializeField] GameObject priceHolder;
    [SerializeField] TextMeshProUGUI stateText;

    public bool isAlreadyOwned {  get; private set; }
    bool isSelected;

    private void Start()
    {
        selected.gameObject.SetActive(true);
        selected.transform.localScale = Vector3.zero;
    }

    public void SetUp(StorePowerData data, StoreUI handler)
    {
        this.data = data;
        this.handler = handler;

        UpdateUI();
    }
    public void UpdateOwnership()
    {
        isAlreadyOwned = PlayerHandler.instance.HasStoreItem(data.storeIndex);


        priceHolder.SetActive(!isAlreadyOwned);
        stateText.gameObject.SetActive(isAlreadyOwned);

        stateText.text = "Bought";

    }

    void UpdateUI()
    {
        nameText.text = data.storeItemName;
        if(data.storeItemSprite != null) icon.sprite = data.storeItemSprite;


        gemSymbol.SetActive(data.currencyType == CurrencyType.Gem);
        goldSymbol.SetActive(data.currencyType == CurrencyType.Coin);
        valueText.text = data.storePrice.ToString();

        UpdateOwnership();
    }

    public override void UpdateAfterBuying()
    {
        base.UpdateAfterBuying();
        UnSelect();
        isAlreadyOwned = true;

        //we need to give that skin to the player but that is done in the handler.

        UpdateUI();

    }

    public void Select()
    {
        isSelected = true;
        float time = 0.1f;
        selected.transform.DOKill();
        selected.transform.DOScale(new Vector3(1.06f, 1.33f, 0), time);
    }
    public void UnSelect()
    {
        isSelected = false;
        float time = 0.15f;
        selected.transform.DOKill();
        selected.transform.DOScale(0, time);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (isSelected && !isAlreadyOwned)
        {
            //then we can buy
            handler.StartBuyItem(data, this);
        }
        else
        {
            //otherwise we just select it and create a description for it somewhere. will do later.
            handler.SelectPower(this);
        }


    }

}


//when you buy it needs to update this fella.
