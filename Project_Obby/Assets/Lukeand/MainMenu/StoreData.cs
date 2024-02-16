using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class StoreData : ScriptableObject
{
    //this will tell teh store button what to do.

    [Separator("STORE")]
    public string storeItemName;
    StoreType storeType;
    public Sprite storeItemSprite;
    public CurrencyType currencyType;
    public int storePrice;   
    [TextArea]public string storeDescription;


    public int storeIndex {  get; private set; }

    public void SetStoreIndex(int newValue)
    {
        storeIndex = newValue;
    }

    protected void BaseBuy()
    {
        //the one thing it always does. is to always infor



    }

    public abstract void Buy();
    public bool CanBuy()
    {
        int certainCurrency = 0;

        if(currencyType == CurrencyType.Gold)
        {
            certainCurrency = PlayerHandler.instance.gold;
        }
        if(currencyType == CurrencyType.Star)
        {
            certainCurrency = PlayerHandler.instance.star;
        }


        return certainCurrency >= storePrice;
    }



    public virtual StorePowerData GetPower() => null;

    public virtual StoreGraphicData GetGraphic() => null;

    public virtual StoreAnimationData GetAnimation() => null;
}
   
public enum StoreType
{
    Power,
    Graphic,
    Animation
}

public enum CurrencyType
{
    Gold,
    Star
}