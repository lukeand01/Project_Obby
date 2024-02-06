using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoreData : ScriptableObject
{
    //this will tell teh store button what to do.

    [Separator("STORE")]
    public string storeItemName;
    public Sprite storeItemSprite;
    public int storePrice;
    [TextArea]public string stageDescription;
    [TextArea]public string storeDescription;



    public abstract void Buy();
    public abstract bool CanBuy();


    public virtual DanceData GetDance() => null;
    public virtual PowerData GetPower() => null;




}
   
