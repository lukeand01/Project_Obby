using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StorePowerUnit : ButtonBase
{
    //whats the difference?
    //how to confirm a purchase? it needs all to be done through a confirmation window.

    //the wya it works is that you se4lected. and if you click again you receive a selection windows.

    StorePowerData data;
    StoreUI handler;

    public void SetUp(StorePowerData data, StoreUI handler)
    {
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
