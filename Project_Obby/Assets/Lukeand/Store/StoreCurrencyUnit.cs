using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoreCurrencyUnit : ButtonBase
{

    StoreData data;
    StoreUI handler;

    [SerializeField] TextMeshProUGUI priceText;

    public void SetUp(StoreData data, StoreUI handler)
    {
        //these in particular might be handled by another sistem
        this.data = data;   
        this.handler = handler;



    }

    public void Selected()
    {

    }
    public void UnSelected()
    {

    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
    }

}
