using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class StoreData : ScriptableObject
{
    //this will tell teh store button what to do.

    [Separator("STORE")]
    public string storeItemName;
    [TextArea]public string storePurchaseDescription;
    StoreType storeType;
    public Sprite storeItemSprite;
    public CurrencyType currencyType;
    public int storePrice;   
    [TextArea]public string storeDescription;

        

    public int storeIndex {  get; private set; } //this needs to be set at the start.

    public void SetStoreIndex(int newValue)
    {
        storeIndex = newValue;
    }

    protected void BaseBuy()
    {
        //the one thing it always does. is to always infor

        if(currencyType == CurrencyType.Coin)
        {
            PlayerHandler.instance.ChangeCoin(-storePrice);
        }
        if(currencyType == CurrencyType.Gem)
        {
            PlayerHandler.instance.ChangeGem(-storePrice);
        }


        if(PlayerHandler.instance != null)
        {
            PlayerHandler.instance.AddStoreItem(storeIndex);
        }
        else
        {
            Debug.Log("there was no playerhandlçer to add this fella");
        }
        
    }

    public abstract void Buy();
    public bool CanBuy()
    {
        int certainCurrency = 0;

        if(currencyType == CurrencyType.Coin)
        {
            certainCurrency = PlayerHandler.instance.coins;
        }
        if(currencyType == CurrencyType.Gem)
        {
            certainCurrency = PlayerHandler.instance.gems;
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
    Coin,
    Gem
}