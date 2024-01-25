using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoreUnit : ButtonBase
{
    //this is a button that buys whatever data is allocated here.

    [SerializeField] StoreData data;

    private void Awake()
    {
        if(data !=  null)
        {

        }
    }

    public void SetUp(StoreData data)
    {

    }

    void UpdateUI()
    {
        SetText(data.name + " - G: " + data.goldPrice);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
    }

}
